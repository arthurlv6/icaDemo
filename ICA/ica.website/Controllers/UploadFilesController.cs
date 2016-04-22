using ica.website.Domain;
using ica.website.Infrastructure;
using ica.website.Models;
using ica.website.Models.UploadFile;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ica.website.Controllers
{
    [Authorize(Roles = "Web Maintenance")]
    public class UploadFilesController : AppFrameController
    {
        IBaseOperations _db;
        ICurrentUser _currentUser;
        public UploadFilesController(IBaseOperations db, ICurrentUser currentUser)
        {
            _db = db;
            _currentUser = currentUser;
        }
        public ActionResult Index(string id= "Undefined")
        {
            var condition = new ConditionViewModel<UserFile>() { ChangeOrderDirection = false, OrderDirection = "desc" };
            if (string.IsNullOrEmpty(id))
            {
                condition.Func = d => d.Deleted != true && d.Category== "Undefined";
            }
            else
            {
                condition.Func = d => d.Deleted != true && d.Category==id;
            }
            condition.PerPageSize = 300;
            condition.CurrentPage = 1;
            var viewModel = new UploadFileViewModel();
            
            viewModel.CategoryList = _db.Get<UserFile>(d=>d.Deleted!=true).GroupBy(d => d.Category).Select(g => new SelectListItem
            {
                Text = g.Key,
                Value = g.Key,
                Selected = g.Key== id
            }).ToArray();
            viewModel.Data = _db.GetSearchResult(condition, d => d.OperationDate, null);
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult Upload(UploadFileViewModel uploadFiles)
        {
            for (int i = 0; i < uploadFiles.Files.Count; i++)
            {
                var file = uploadFiles.Files[i];
                if (file == null) continue;
                var fileName = Path.GetFileName(file.FileName);
                if (string.IsNullOrEmpty(fileName)) return RedirectToAction("Index");
                var ext = fileName.Substring(fileName.IndexOf(".")).ToLower();
                if (ext == ".jpg" || ext == ".png" || ext == ".bmp")
                {
                    if (file.ContentLength > 1000000) continue;
                }
                
                var newImage = new UserFile
                {
                    OperationDate = _db.GetNzTime(),
                    Operator = _currentUser.User.Email,
                    FileType = ext,
                    Category=uploadFiles.Category,
                    Name=fileName
                };
                _db.AddOneEntity(newImage);
                _db.SaveChange();
                if (ext == ".jpg" || ext == ".png" || ext == ".bmp")
                {
                    file.SaveAs(Server.MapPath("~/images/FileLibrary/" + newImage.Id + ext));
                }
                else
                {
                    file.SaveAs(Server.MapPath("~/images/FileLibrary/" + newImage.Name));
                }
            }
            return RedirectToAction<UploadFilesController>(d => d.Index(uploadFiles.Category));
        }
        public JsonResult _Delete(long id)
        {
            try
            {
                _db.FindDetail<UserFile>(id).Deleted = true;
                _db.SaveChange();
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("fail", JsonRequestBehavior.AllowGet);
            }
        }
    }
}