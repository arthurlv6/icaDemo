using ica.website.Domain;
using ica.website.Infrastructure;
using ica.website.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ica.website.Controllers
{
    [Authorize(Roles = "Web Maintenance")]
    public class WebDepartmentController : AppFrameController
    {
        private readonly IBaseOperations _context;
        private readonly ICurrentUser _currentUser;
        public WebDepartmentController(IBaseOperations context, ICurrentUser currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }
        public ActionResult Index(Int64 languageId = 1)
        {
            ViewBag.selected = languageId;
            return View(_context.Get<WebDepartment>(d => d.LanguageId == languageId));
        }

        [HttpPost]
        public ActionResult Index(FormCollection input)
        {
            var IdArray = input.GetValues("item.Id");
            var NameArray = input.GetValues("item.Name");
            var OrderArray = input.GetValues("item.Order");
            long lanaguageId = 1;
            for (int i = 0; i < IdArray.Count(); i++)
            {
                var Id = Convert.ToInt64(IdArray[i]);
                var entity = _context.FindDetail<WebDepartment>(Id);
                entity.Name = NameArray[i];
                entity.Order = int.Parse(OrderArray[i] == "" ? "10" : OrderArray[i]);
                lanaguageId = entity.LanguageId;
                
            }
            _context.SaveChange();
            return RedirectToAction<WebDepartmentController>(d => d.Index(lanaguageId));
        }
        [HttpPost]
        public ActionResult Add(WebDepartment input)
        {
            _context.AddOneEntity<WebDepartment>(input);
            return RedirectToAction<WebDepartmentController>(d => d.Index(input.LanguageId));
        }
        public JsonResult _Delete(long id)
        {
            try
            {
                _context.DeleteOneEntity<WebDepartment>(id);
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("fail", JsonRequestBehavior.AllowGet);
            }
        }
    }
}
