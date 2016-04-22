using ica.website.Domain;
using ica.website.Infrastructure;
using ica.website.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ica.website.Controllers
{
    [Authorize(Roles = "Web Maintenance")]
    public class CollegeController : AppFrameController
    {
        private readonly IBaseOperations _context;
        private readonly ICurrentUser _currentUser;
        public CollegeController(IBaseOperations context,ICurrentUser currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }
        public ActionResult Index(Int64 languageId = 1)
        {
            ViewBag.selected = languageId;
            return View(_context.Get<College>(d => d.LanguageId==languageId).FirstOrDefault());
        }
        [HttpPost,ValidateInput(false)]
        public ActionResult Save (College input)
        {
            _context.UpdateOneEntity<College>(input);
            return RedirectToAction<CollegeController>(d => d.Index(input.LanguageId));
        }
        public ActionResult UploadImage(FormCollection form)
        {
            HttpPostedFileBase file = Request.Files[0] as HttpPostedFileBase;
            var imageName = form.GetValues("imageName")[0];
            var languageId = form.GetValues("languageIdForImage")[0];
            string pic = "";
            if (file != null)
            {
                var fileName = Path.GetFileName(file.FileName);
                var ext = fileName.Substring(fileName.IndexOf(".") + 1).ToLower();
                if (ext != "gif" && ext == "png" && ext == "jpg" && ext == "bmp")
                {
                    return Json("Sorry, you can only upload gif,png,jpg or bmp file.", JsonRequestBehavior.AllowGet);
                }
                if (file.ContentLength > 200000)//200k
                {
                    return Json("Sorry, your logo file size can not be bigger than 400k. Please upload proper size file.", JsonRequestBehavior.AllowGet);
                }
                using (System.Drawing.Image image = System.Drawing.Image.FromStream(file.InputStream, true, true))
                {
                    if (imageName == "Icon")
                    {
                        if (image.Width != 260 || image.Height != 130)
                        {
                            return Json("Sorry, file dimensions is wrong.", JsonRequestBehavior.AllowGet);
                        }
                    }
                    pic = Guid.NewGuid().ToString() + "." + ext;
                    string path = System.IO.Path.Combine(Server.MapPath("/images/Main/"), pic);
                    file.SaveAs(path);
                    var college = _context.FindDetail<College>(Convert.ToInt64(languageId));
                    var pathForDatabase = "/images/Main/" + pic;
                    if (imageName == "Icon")
                    {
                        college.Icon = pathForDatabase;
                    }
                    if (imageName == "Emphasis")
                    {
                        college.EmphasisImage = pathForDatabase;
                    }
                    if (imageName == "Introduction")
                    {
                        college.IntroductionImage = pathForDatabase;
                    }
                    if (imageName == "Dean")
                    {
                        college.DeanImage = pathForDatabase;
                    }
                    if (imageName == "Bar")
                    {
                        college.FootBarImage = pathForDatabase;
                    }
                    _context.SaveChange();
                    return Json("done"+imageName+ pathForDatabase, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json("Please select your file", JsonRequestBehavior.AllowGet);
            }
        }
    }
}