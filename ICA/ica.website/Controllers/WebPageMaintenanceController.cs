using ica.website.Infrastructure;
using ica.website.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace ica.website.Controllers
{
    public class WebPageMaintenanceController : AppFrameController
    {
        [Authorize(Roles = "Web Maintenance")]
        public ActionResult SelectTable(string id)
        {
            if (id == "NewsType")
                return RedirectToAction<WebNewsTypesController>(c => c.Index(1));
            if (id == "News")
                return RedirectToAction<WebNewsController>(c => c.Index(1));
            if (id == "Alumni")
                return RedirectToAction<AlumniController>(c => c.Index());
            if (id == "Events")
                return RedirectToAction<WebEventController>(c => c.Index(1));
            if (id == "Languages")
                return RedirectToAction<WebLanguageController>(c => c.Index());
            if (id == "Stories")
                return RedirectToAction<WebStoryController>(c => c.Index(1));
            if (id == "MainMenu")
                return RedirectToAction<WebMenuController>(c => c.Index(1));
            if (id == "College")
                return RedirectToAction<CollegeController>(c => c.Index(1));
            if (id == "Carousel")
                return RedirectToAction<WebCarouselController>(c => c.Index(1));
            if (id == "JobType")
                return RedirectToAction<WebJobTypeController>(c => c.Index(1));
            if (id == "Staff")
                return RedirectToAction<WebStaffController>(c => c.Index(1));
            if (id == "Department")
                return RedirectToAction<WebDepartmentController>(c => c.Index(1));
            if (id == "CarouselBackground")
                return RedirectToAction<WebCarouselBackgroundController>(c=>c.Index());
            if (id == "FileLibrary")
                return RedirectToAction<UploadFilesController>(c => c.Index("Undefined"));
            return View();
        }
    }
}