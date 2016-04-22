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
    public class WebCarouselBackgroundController : AppFrameController
    {
        private readonly IBaseOperations _context;
        private readonly ICurrentUser _currentUser;
        public WebCarouselBackgroundController(IBaseOperations context, ICurrentUser currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }
        public ActionResult Index()
        {
            return View(_context.Get<WebCarouselBackground>(d => true));
        }
        public ActionResult UploadImage()
        {
            HttpPostedFileBase file = Request.Files[0] as HttpPostedFileBase;
            string pic = "";
            if (file != null)
            {
                using (System.Drawing.Image image = System.Drawing.Image.FromStream(file.InputStream, true, true))
                {
                    if (image.Width == 1385 && image.Height == 398)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var ext = fileName.Substring(fileName.IndexOf(".") + 1).ToLower();
                        if (ext != "gif" && ext == "png" && ext == "jpg" && ext == "bmp")
                        {
                            return Json("Sorry, you can only upload gif,png,jpg or bmp file.", JsonRequestBehavior.AllowGet);
                        }
                        if (file.ContentLength > 400000)//400k
                        {
                            return Json("Sorry, your logo file size can not be bigger than 400k. Please upload proper size file.", JsonRequestBehavior.AllowGet);
                        }
                        pic = Guid.NewGuid().ToString() + "." + ext;
                        string path = System.IO.Path.Combine(Server.MapPath("/images/Slider/"), pic);
                        file.SaveAs(path);
                        _context.AddOneEntity<WebCarouselBackground>(new WebCarouselBackground { Name = fileName, Path = "/images/Slider/" + pic });
                        return Json("done", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json("Sorry, file dimensions is wrong.", JsonRequestBehavior.AllowGet);
                    }
                }
            }
            else
            {
                return Json("Please select your file", JsonRequestBehavior.AllowGet);
            }
        }
    }
}