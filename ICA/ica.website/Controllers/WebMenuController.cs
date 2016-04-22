using ica.website.Infrastructure;
using ica.website.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ica.website.Domain;
using System.Data.Entity;

namespace ica.website.Controllers
{
    [Authorize(Roles = "Web Maintenance")]
    public class WebMenuController : AppFrameController
    {
        // GET: Alumni
        private readonly IBaseOperations _context;
        private readonly ICurrentUser _currentUser;
        public WebMenuController(IBaseOperations context, ICurrentUser currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }
        public ActionResult Index(long languageId=1)
        {
            ViewBag.selected = languageId;
            return View(_context.Get<WebMenu>(d => d.LanguageId==languageId).Include("WebLanguage"));
        }
        [HttpPost ValidateInput(false)]
        public ActionResult SaveAll(FormCollection input)
        {
            var IdArray = input.GetValues("item.Id");
            var NameArray = input.GetValues("item.Name");
            var LinkArray = input.GetValues("item.Link");
            var OrderArray = input.GetValues("item.Order");
            long lanaguageId = 1;
            for (int i = 0; i < IdArray.Count(); i++)
            {
                var Id = Convert.ToInt64(IdArray[i]);
                var entity=_context.FindDetail<WebMenu>(Id);
                entity.Name = NameArray[i];
                entity.Link = LinkArray[i];
                int order = 0;
                bool res = int.TryParse(OrderArray[i], out order);
                entity.Order = order;
                lanaguageId = entity.LanguageId;
            }
            _context.SaveChange();
            return RedirectToAction<WebMenuController>(d => d.Index(lanaguageId));
        }
       
        public ActionResult EditContent(long id)
        {
            return View(_context.FindDetail<WebMenu>(id));
        }
        [HttpPost ValidateInput(false)]
        public ActionResult SaveContent(WebMenu input)
        {
            var temp = _context.FindDetail<WebMenu>(input.Id);
            temp.Content = input.Content;
            temp.UpdatedDate = DateTime.Now;
            _context.SaveChange();
            return RedirectToAction<WebMenuController>(d => d.Index(temp.LanguageId));
        }
        public JsonResult _Add(long parentId,long languageId,string name)
        {
            long? menuParentId;
            if (parentId == 0)
            {
                menuParentId = null;
            }
            else
            {
                menuParentId = parentId;
            }
            var item = new WebMenu { ParentId = menuParentId, LanguageId = languageId, Name = name ,UpdatedDate=DateTime.Now};
            _context.AddOneEntity<WebMenu>(item);
            return Json("success", JsonRequestBehavior.AllowGet);
        }
        public JsonResult _Delete(long id)
        {
            try
            {
                foreach (var item in _context.Get<WebMenu>(d => d.ParentId == id).ToList())
                {
                    foreach (var subItem in _context.Get<WebMenu>(d => d.ParentId == item.Id).ToList())
                    {
                        _context.DeleteOneEntity<WebMenu>(subItem.Id);
                    }
                    _context.DeleteOneEntity<WebMenu>(item.Id);
                }
                _context.DeleteOneEntity<WebMenu>(id);
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("fail", JsonRequestBehavior.AllowGet);
            }
        }
    }
}