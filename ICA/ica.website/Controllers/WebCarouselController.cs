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
    public class WebCarouselController : AppFrameController
    {
        private readonly IBaseOperations _context;
        private readonly ICurrentUser _currentUser;
        public WebCarouselController(IBaseOperations context, ICurrentUser currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }
        public ActionResult Index(Int64 languageId = 1)
        {
            ViewBag.selected = languageId;
            return View(_context.Get<WebCarousel>(d => d.LanguageId == languageId));
        }

        [HttpPost ValidateInput(false)]
        public ActionResult Index(FormCollection input)
        {
            var IdArray = input.GetValues("item.Id");
            var WordsArray = input.GetValues("item.Words");
            var BackgroudImageArray = input.GetValues("item.BackgroudImage");
            var IndicateHeaderArray = input.GetValues("item.IndicateHeader");
            var IndicateWordsArray = input.GetValues("item.IndicateWords");
            var OrderArray = input.GetValues("item.Order");
            //var IssueArray = input.GetValues("item.Issue");
            var ContentArray = input.GetValues("item.Content");
            long lanaguageId = 1;
            for (int i = 0; i < IdArray.Count(); i++)
            {
                var Id = Convert.ToInt64(IdArray[i]);
                var entity = _context.FindDetail<WebCarousel>(Id);
                lanaguageId = entity.LanguageId;
                entity.Words = WordsArray[i];
                entity.Content = ContentArray[i];
                entity.BackgroudImage = BackgroudImageArray[i];
                entity.Order = int.Parse(OrderArray[i]);
                entity.UpdatedDate = DateTime.Now;
                entity.IndicateHeader = IndicateHeaderArray[i];
                entity.IndicateWords = IndicateWordsArray[i];
                //entity.Issue = bool.Parse(IssueArray[i]);
            }
            _context.SaveChange();
            return RedirectToAction<WebCarouselController>(d => d.Index(lanaguageId));
        }
       
        public ActionResult UploadImage(HttpPostedFileBase file,FormCollection form)
        {
            var alumniId = form.GetValues("alumniId")[0];
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
                    return Json("Sorry, your logo file size can not be bigger than 200k. Please upload proper size file.", JsonRequestBehavior.AllowGet);
                }
                pic = Guid.NewGuid().ToString() + "." + ext;
                string path = System.IO.Path.Combine(Server.MapPath("/images/Alumni/"), pic);
                // file is uploaded
                file.SaveAs(path);
                _context.FindDetail<ApplicationUser>(alumniId).HeaderImage = "/images/Alumni/" + pic;
                _context.SaveChange();
                return Json("alert('Seccussfull.')", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("alert('Please select your logo file.')", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Add(WebCarousel input)
        {
            input.UpdatedDate = DateTime.Now;
            _context.AddOneEntity<WebCarousel>(input);
            return RedirectToAction<WebCarouselController>(d => d.Index(input.LanguageId));
        }
        public JsonResult _Delete(long id)
        {
            try
            {
                _context.DeleteOneEntity<WebCarousel>(id);
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("fail", JsonRequestBehavior.AllowGet);
            }
        }
    }
}