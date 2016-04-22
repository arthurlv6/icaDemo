using ica.website.Domain;
using ica.website.Infrastructure;
using ica.website.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace ica.website.Controllers
{
    [Authorize(Roles = "Web Maintenance")]
    public class WebNewsController : AppFrameController
    {
        private readonly IBaseOperations _context;
        private readonly ICurrentUser _currentUser;
        public WebNewsController(IBaseOperations context, ICurrentUser currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }
        public ActionResult Index(long? languageId = 1)
        {
            ViewBag.selected = languageId;
            var modal = _context.Get<WebNews>(d => d.WebNewsType.LanguageId == languageId).OrderByDescending(d => d.Id).Include(d => d.WebNewsType).ToList();
            return View(modal);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Index(FormCollection input)
        {
            var IdArray = input.GetValues("item.Id");
            var NameArray = input.GetValues("item.Name");
            var ImageArray = input.GetValues("item.Image");
            var ContentArray = input.GetValues("item.Content");
            var DateArray = input.GetValues("item.Date");
            var ShortDescriptionArray = input.GetValues("item.ShortDescription");
            
            var WebNewsTypeIdArray = input.GetValues("item.WebNewsTypeId");

            long lanaguageId = 1;
            var LanguageIdArray = input.GetValues("item.LanguageId");
            if (LanguageIdArray.Count() > 0)
            {
                lanaguageId = Convert.ToInt64(LanguageIdArray[0]);
            }

            for (int i = 0; i < IdArray.Count(); i++)
            {
                var Id = Convert.ToInt64(IdArray[i]);
                var entity = _context.FindDetail<WebNews>(Id);
                entity.Name = NameArray[i];
                entity.WebNewsTypeId = Convert.ToInt64(WebNewsTypeIdArray[i]);
                entity.Date= Convert.ToDateTime(DateArray[i]);
                var content = ContentArray[i];
                entity.Image = _context.GetImage(content, "news");
                entity.ShortDescription = ShortDescriptionArray[i];
                entity.Content = content;
                entity.UpdatedDate = DateTime.Now;
            }
            _context.SaveChange();
            return RedirectToAction<WebNewsController>(d => d.Index(lanaguageId));
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Add(WebNews input)
        {
            input.Image = _context.GetImage(input.Content, "news");
            input.UpdatedDate = DateTime.Now;
            _context.AddOneEntity<WebNews>(input);
            return RedirectToAction<WebNewsController>(d => d.Index(input.LanguageId));
        }
        public JsonResult _Delete(long id)
        {
            try
            {
                _context.DeleteOneEntity<WebNews>(id);
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("fail", JsonRequestBehavior.AllowGet);
            }
        }
    }
}
