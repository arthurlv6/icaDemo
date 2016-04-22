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
    public class WebEventController : AppFrameController
    {
        private readonly IBaseOperations _context;
        private readonly ICurrentUser _currentUser;
        public WebEventController(IBaseOperations context, ICurrentUser currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }
        public ActionResult Index(long languageId = 1)
        {
            ViewBag.selected = languageId;
            return View(_context.Get<WebEvent>(d => d.LanguageId == languageId).OrderByDescending(d=>d.Id));
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Index(FormCollection input)
        {
            var IdArray = input.GetValues("item.Id");
            var NameArray = input.GetValues("item.Name");
            var ImageArray = input.GetValues("item.Image");
            var ContentArray = input.GetValues("item.Content");
            var AddressArray = input.GetValues("item.Address");
            var DateArray = input.GetValues("item.Date");
            var DuringArray = input.GetValues("item.During");

            long lanaguageId = 1;
            for (int i = 0; i < IdArray.Count(); i++)
            {
                var Id = Convert.ToInt64(IdArray[i]);
                var entity = _context.FindDetail<WebEvent>(Id);
                entity.Name = NameArray[i];
                entity.Address = AddressArray[i];
                entity.Date = Convert.ToDateTime(DateArray[i]);
                entity.During = DuringArray[i];
                var content = ContentArray[i];
                entity.Image = _context.GetImage(content, "event");
                entity.Content = content;
                entity.UpdatedDate = DateTime.Now;
            }
            _context.SaveChange();
            return RedirectToAction<WebEventController>(d => d.Index(lanaguageId));
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Add(WebEvent input)
        {
            input.Image = _context.GetImage(input.Content,"event");
            input.UpdatedDate = DateTime.Now;
            _context.AddOneEntity<WebEvent>(input);
            return RedirectToAction<WebEventController>(d => d.Index(input.LanguageId));
        }
        public JsonResult _Delete(long id)
        {
            try
            {
                _context.DeleteOneEntity<WebEvent>(id);
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("fail", JsonRequestBehavior.AllowGet);
            }
        }
    }
}
