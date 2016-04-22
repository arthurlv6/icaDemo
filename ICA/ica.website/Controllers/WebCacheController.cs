using ica.website.App_Start;
using ica.website.Infrastructure;
using ica.website.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ica.website.Controllers
{
    [Authorize(Roles = "Cache Mangement")]
    public class WebCacheController : AppFrameController
    {
        // GET: WebCache
        public ActionResult Index(string id="")
        {
            ViewBag.message = id;
            return View();
        }
        public ActionResult Refresh(string id)
        {
            if (id == "all")
            {
                new RunAtStartup().Execute();
            }
            else
            {
                Type className=Type.GetType(id);
                //CacheHelper.GetCache<className>
            }
            return RedirectToAction<WebCacheController>(d => d.Index(id));
        }
    }
}