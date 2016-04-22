using ica.website.Domain;
using ica.website.Infrastructure;
using ica.website.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace ica.website.Controllers
{
    [Authorize(Roles = "Web Maintenance")]
    public class WebStaffController : AppFrameController
    {
        private readonly IBaseOperations _context;
        private readonly ICurrentUser _currentUser;
        public WebStaffController(IBaseOperations context, ICurrentUser currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }
        public ActionResult Index(long? languageId = 1)
        {
            ViewBag.selected = languageId;
            var modal = _context.Get<WebStaff>(d => d.LanguageId == languageId).OrderByDescending(d => d.Id).Include(d=>d.WebDepartment).ToList();
            return View(modal);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Index(FormCollection input)
        {
            var IdArray = input.GetValues("item.Id");
            var NameArray = input.GetValues("item.Name");
            var ImageArray = input.GetValues("item.Image");
            var ContentArray = input.GetValues("item.Content");
            var PhoneArray = input.GetValues("item.Phone");
            var EmailArray = input.GetValues("item.Email");
            var PositonArray = input.GetValues("item.Position");
            var DepartmentIdArray = input.GetValues("item.WebDepartmentId");
            var Order = input.GetValues("item.Order");

            long lanaguageId = 1;
            var LanguageIdArray = input.GetValues("item.LanguageId");
            if (LanguageIdArray.Count() > 0)
            {
                lanaguageId = Convert.ToInt64(LanguageIdArray[0]);
            }
            
            for (int i = 0; i < IdArray.Count(); i++)
            {
                var Id = Convert.ToInt64(IdArray[i]);
                var entity = _context.FindDetail<WebStaff>(Id);
                entity.Name = NameArray[i];
                entity.Phone = PhoneArray[i];
                entity.Email = EmailArray[i];
                entity.Position = PositonArray[i];
                entity.WebDepartmentId = Convert.ToInt64(DepartmentIdArray[i]);
                var content = ContentArray[i];
                entity.Image = _context.GetImage(content, "staff");
                entity.Content = content;
                entity.Order = int.Parse(Order[i] == ""? "10": Order[i]);
                entity.UpdatedDate = DateTime.Now;
            }
            _context.SaveChange();
            return RedirectToAction<WebStaffController>(d => d.Index(lanaguageId));
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Add(WebStaff input)
        {
            input.Image = _context.GetImage(input.Content, "staff");
            input.UpdatedDate = DateTime.Now;
            _context.AddOneEntity<WebStaff>(input);
            return RedirectToAction<WebStaffController>(d => d.Index(input.LanguageId));
        }
        public JsonResult _Delete(long id)
        {
            try
            {
                _context.DeleteOneEntity<WebStaff>(id);
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("fail", JsonRequestBehavior.AllowGet);
            }
        }
    }
}
