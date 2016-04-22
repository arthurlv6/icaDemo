using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ica.website.Domain;
using ica.website.Models;
using ica.website.Infrastructure;

namespace ica.website.Controllers
{
    [Authorize(Roles = "Web Maintenance")]
    public class WebNewsTypesController : AppFrameController
    {
        private readonly IBaseOperations _context;
        private readonly ICurrentUser _currentUser;
        public WebNewsTypesController(IBaseOperations context, ICurrentUser currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }
        public ActionResult Index(Int64 languageId = 1)
        {
            ViewBag.selected = languageId;
            return View(_context.Get<WebNewsType>(d => d.LanguageId == languageId));
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
                var entity = _context.FindDetail<WebNewsType>(Id);
                entity.Name = NameArray[i];
                entity.Order = int.Parse(OrderArray[i] == "" ? "10":OrderArray[i]);
                lanaguageId = entity.LanguageId;
            }
            _context.SaveChange();
            return RedirectToAction<WebNewsTypesController>(d => d.Index(lanaguageId));
        }
        [HttpPost]
        public ActionResult Add(WebNewsType input)
        {
            _context.AddOneEntity<WebNewsType>(input);
            return RedirectToAction<WebNewsTypesController>(d => d.Index(input.LanguageId));
        }
        public JsonResult _Delete(long id)
        {
            try
            {
                _context.DeleteOneEntity<WebNewsType>(id);
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("fail", JsonRequestBehavior.AllowGet);
            }
        }
    }
}
