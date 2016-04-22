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
    public class WebStoryController : AppFrameController
    {
        private readonly IBaseOperations _context;
        private readonly ICurrentUser _currentUser;
        public WebStoryController(IBaseOperations context, ICurrentUser currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }
        public ActionResult Index(long? languageId = 1)
        {
            ViewBag.selected = languageId;
            var modal = _context.Get<WebStory>(d => d.LanguageId == languageId).OrderByDescending(d => d.Id).Include(d => d.WebDepartment).ToList();
            return View(modal);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Index(FormCollection input)
        {
            var IdArray = input.GetValues("item.Id");
            var NameArray = input.GetValues("item.Name");
            var ContentArray = input.GetValues("item.Content");
            var DepartmentIdArray = input.GetValues("item.WebDepartmentId");
            var ShortDescriptionArray = input.GetValues("item.ShortDescription");
            var OrderArray = input.GetValues("item.Order");
            long lanaguageId = 1;
            var LanguageIdArray = input.GetValues("item.LanguageId");
            if (LanguageIdArray.Count() > 0)
            {
                lanaguageId = Convert.ToInt64(LanguageIdArray[0]);
            }

            for (int i = 0; i < IdArray.Count(); i++)
            {
                var Id = Convert.ToInt64(IdArray[i]);
                var entity = _context.FindDetail<WebStory>(Id);
                entity.Name = NameArray[i];
                entity.WebDepartmentId = Convert.ToInt64(DepartmentIdArray[i]);
                var content = ContentArray[i];
                entity.Image = _context.GetImage(content, "staff");
                entity.Content = content;
                entity.ShortDescription = ShortDescriptionArray[i];
                entity.UpdatedDate = DateTime.Now;
                entity.Order = int.Parse(OrderArray[i] == "" ? "5": OrderArray[i]);
            }
            _context.SaveChange();
            return RedirectToAction<WebStoryController>(d => d.Index(lanaguageId));
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Add(WebStory input)
        {
            input.Image = _context.GetImage(input.Content, "story");
            input.UpdatedDate = DateTime.Now;
            input.Order = 5;
            _context.AddOneEntity<WebStory>(input);
            return RedirectToAction<WebStoryController>(d => d.Index(input.LanguageId));
        }
        public JsonResult _Delete(long id)
        {
            try
            {
                _context.DeleteOneEntity<WebStory>(id);
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("fail", JsonRequestBehavior.AllowGet);
            }
        }
    }
}