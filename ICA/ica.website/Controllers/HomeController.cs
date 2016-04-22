using Boilerplate.Web.Mvc;
using Boilerplate.Web.Mvc.Filters;
using ica.website.Domain;
using ica.website.infrastructure;
using ica.website.Infrastructure;
using ica.website.Models;
using ica.website.Models.MainPage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ica.website.Controllers
{
    [RoutePrefix("Home")]
    public class HomeController : AppFrameController
    {
        private readonly IBaseOperations _context;
        public HomeController(IBaseOperations context)
        {
            _context = context;
        }
        #region
        //[OutputCache(Duration = 60,VaryByParam ="cache")]// after 3 minutesa
        [Route("~/", Name = "HomeGetIndex")]
        public ActionResult Index()
        {
            //throw new Exception();
            ViewBag.Title = "ICA";
            MainPageViewModel mainData = new MainPageViewModel();
            var languageId = getLanguageId();
            mainData.College = CacheHelper.GetCache<College>(CacheType.College).Where(d => d.LanguageId == languageId).FirstOrDefault();
            mainData.Events = CacheHelper.GetCache<WebEvent>(CacheType.WebEvent).Where(d => d.LanguageId == languageId).OrderByDescending(d=>d.Date).Take(5).Select(d=>new WebEvent {Id=d.Id,Date=d.Date,Name=d.Name,Address=d.Address,During=d.During,Image=d.Image }).ToList();
            mainData.News = CacheHelper.GetCache<WebNews>(CacheType.WebNews).Where(d => d.LanguageId == languageId).OrderByDescending(d=>d.Date).Take(5).Select(d=>new WebNews { Id=d.Id,Name=d.Name,Image=d.Image,ShortDescription=d.ShortDescription}).ToList();
            mainData.Stories = CacheHelper.GetCache<WebStory>(CacheType.WebStory).Where(d => d.LanguageId == languageId).OrderBy(d=>d.Order).Take(5).Select(d=>new WebStory {Id= d.Id,ShortDescription= d.ShortDescription,Name= d.Name,Image= d.Image}).ToList();
            mainData.Links= CacheHelper.GetCache<WebUsefulLink>(CacheType.WebUsefulLink).Where(d => d.LanguageId == languageId).Take(5).ToList();
            mainData.Carousels = CacheHelper.GetCache<WebCarousel>(CacheType.WebCarousel).Where(d => d.LanguageId == languageId).OrderBy(d=>d.Order).ToList();
            return View(mainData);
        }
        public ActionResult Search(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return RedirectToAction<HomeController>(d => d.Index());
            }
            SearchResultViewModel result = new SearchResultViewModel();
            var languageId = getLanguageId();
            result.SearchWords = input;
            result.Events= CacheHelper.GetCache<WebEvent>(CacheType.WebEvent).Where(d => d.Content.Contains( input)).Where(d=>d.LanguageId==languageId).OrderByDescending(d => d.Date).Take(5).ToList();
            result.News= CacheHelper.GetCache<WebNews>(CacheType.WebNews).Where(d => d.Content.Contains(input) || d.ShortDescription.Contains(input)).Where(d => d.LanguageId == languageId).OrderByDescending(d => d.Date).Take(5).ToList();
            result.Stories= CacheHelper.GetCache<WebStory>(CacheType.WebStory).Where(d => d.Content.Contains(input) || d.ShortDescription.Contains(input)).Where(d => d.LanguageId == languageId).OrderByDescending(d => d.Id).Take(5).ToList();
            result.Staff = CacheHelper.GetCache<WebStaff>(CacheType.WebStaff).Where(d => d.Content.Contains(input)).Where(d => d.LanguageId == languageId).OrderByDescending(d => d.Id).Take(5).ToList();
            result.Menus = CacheHelper.GetCache<WebMenu>(CacheType.WebMenu).Where(d=>d.Content!=null).Where(d => d.Content.Contains(input) || d.Name.Contains(input)).Where(d => d.LanguageId == languageId).OrderByDescending(d => d.Id).Take(5).ToList();
            return View(result);
        }
        [ChildActionOnly]
        public ActionResult Menu()
        {
            var college= CacheHelper.GetCache<College>(CacheType.College).Where(d => d.LanguageId == 1).FirstOrDefault();
            var menu = _context.Get<WebMenu>(d => d.WebLanguage.Name == "English").ToList();
                //CacheHelper.GetCache<WebMenu>(CacheType.WebMenu).Where(d => d.LanguageId == 1).ToList();

            var temp = new MainPageMenuViewModel();
            temp.College = college;
            temp.WebMenus = menu;
            
            return View(temp);
        }
        public ActionResult Chat()
        {
            return View();
        }
        [Route("ModalCarouselDetail", Name = "HomeGetModalCarouselDetail")]
        public ActionResult ModalCarouselDetail(long Id)
        {
            var temp = CacheHelper.GetCache<WebCarousel>(CacheType.WebCarousel).Where(d=>d.Id==Id).FirstOrDefault();
            return PartialView("_ModalCarouselDetail", temp);
        }
        [Route("ModalStoryDetail", Name = "HomeGetModalStoryDetail")]
        public ActionResult ModalStoryDetail(long Id)
        {
            var temp = CacheHelper.GetCache<WebStory>(CacheType.WebStory).Where(d => d.Id == Id).FirstOrDefault();
            return PartialView("_ModalStoryDetail", temp);
        }

        [Route("ModalStaffDetail", Name = "HomeGetModalStaffDetail")]
        public ActionResult ModalStaffDetail(long Id)
        {
            var temp = CacheHelper.GetCache<WebStaff>(CacheType.WebStaff).Where(d=>d.Id==Id).FirstOrDefault();
            return PartialView("_ModalStaffDetail", temp);
        }

        [Route("ModalMainpageDetail", Name = "HomeGetModalMainpageDetail")]
        public ActionResult ModalMainpageDetail(string id)
        {
            long languageId = getLanguageId();
            var temp = CacheHelper.GetCache<College>(CacheType.College).Where(d => d.LanguageId == languageId).FirstOrDefault();
            if (id == "Introduction")
            {
                return PartialView("_ModalMainpageDetail", temp.Introduction);
            }
            else if(id == "DeanMessage")
            {
                return PartialView("_ModalMainpageDetail", temp.DeanMessage);
            }
            else if (id == "FootNav1")
            {
                return PartialView("_ModalMainpageDetail", temp.FootNav1Content);
            }
            else if (id == "FootNav2")
            {
                return PartialView("_ModalMainpageDetail", temp.FootNav2Content);
            }
            else if (id == "FootNav3")
            {
                return PartialView("_ModalMainpageDetail", temp.FootNav3Content);
            }
            else if (id == "FootNav4")
            {
                return PartialView("_ModalMainpageDetail", temp.FootNav4Content);
            }
            else if (id == "Terms")
            {
                return PartialView("_ModalMainpageDetail", temp.Terms);
            }
            else if (id == "Policy")
            {
                return PartialView("_ModalMainpageDetail", temp.Policy);
            }
            else
            {
                return PartialView("_ModalComing", "Please contact administrator.");
            }
        }

        [Route("ModalNewsDetail", Name = "HomeGetModalNewsDetail")]
        public ActionResult ModalNewsDetail(long Id)
        {
            var temp = CacheHelper.GetCache<WebNews>(CacheType.WebNews).Where(d => d.Id == Id).FirstOrDefault();
            return PartialView("_ModalNews", temp);
        }

        [Route("ModalEventDetail", Name = "HomeGetModalEventDetail")]
        public ActionResult ModalEventDetail(long Id)
        {
            var temp = CacheHelper.GetCache<WebEvent>(CacheType.WebEvent).Where(d => d.Id == Id).FirstOrDefault();
            return PartialView("_ModalEvent", temp);
        }
        
        public ActionResult ModalComing(string Id)
        {
            return PartialView("_ModalComing", Id);
        }

        [Route("ModalMenuItem", Name = "HomeGetModalMenuItem")]
        public ActionResult ModalMenuItem(long Id)
        {
            var temp = CacheHelper.GetCache<WebMenu>(CacheType.WebMenu).Where(d => d.Id == Id).FirstOrDefault();
            return PartialView("_ModalMenuItem", temp);
        }
        #endregion
        public ActionResult ChangeLanguage(long LanguageId)
        {
            if (Request.Cookies["Company"] != null)
            {
                Response.Cookies.Remove("Company");
            }
            var cookie = new HttpCookie("Company");
            cookie.Values.Add("languageId", LanguageId.ToString());
            cookie.Expires = DateTime.Now.AddDays(15);
            Response.Cookies.Add(cookie);
            return RedirectToAction<HomeController>(c=>c.Index());
        }
        #region learn more
        [Route("News", Name = "HomeGetNews")]
        public ActionResult News()
        {
            var languageId = getLanguageId();
            var model = CacheHelper.GetCache<WebNewsType>(CacheType.WebNewsTypeData).Where(d => d.LanguageId == languageId).OrderBy(d=>d.Order).ToList();
            return View(model);
        }
        [Route("Staff", Name = "HomeGetStaff")]
        public ActionResult Staff()
        {
            var languageId = getLanguageId();
            var model = CacheHelper.GetCache<WebDepartment>(CacheType.WebDepartmentStaffData).Where(d => d.LanguageId == languageId).OrderBy(D=>D.Order).ToList();
            return View(model);
        }
        [Route("Events", Name = "HomeGetEvents")]
        public ActionResult Events()
        {
            var languageId = getLanguageId();
            var model = CacheHelper.GetCache<WebEvent>(CacheType.WebEvent).Where(d => d.LanguageId == languageId).OrderByDescending(d => d.Date).ToList();
            return View(model);
        }
        [Route("Stories", Name = "HomeGetStories")]
        public ActionResult Stories()
        {
            var languageId = getLanguageId();
            var model = CacheHelper.GetCache<WebDepartment>(CacheType.WebDepartmentStoryData).Where(d => d.LanguageId == languageId).OrderByDescending(d => d.Id).ToList();
            return View(model);
        }
        #endregion
        #region SEO
        [NoTrailingSlash]
        [Route("~/sitemap.xml", Name = "GetSitemapXml")]
        public ActionResult SitemapXml(int? index = null)
        {
            var sitemapService = new SitemapService(this.Url);
            string content = sitemapService.GetSitemapXml(index);

            if (content == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Sitemap index is out of range.");
            }

            return this.Content(content, ContentType.Xml, Encoding.UTF8);
        }
        [Route("~/robots.txt", Name = "GetRobotsText"), OutputCache(Duration = 86400)]
        public ContentResult RobotsText()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("user-agent: *");
            stringBuilder.AppendLine("disallow: /webcache/");
            stringBuilder.AppendLine("disallow: /webCarouselBackground/");
            stringBuilder.AppendLine("disallow: /webCarousel/");
            stringBuilder.AppendLine("disallow: /webDepartment/");
            stringBuilder.AppendLine("disallow: /webEvent/");
            stringBuilder.AppendLine("disallow: /webJobType/");
            stringBuilder.AppendLine("disallow: /webLanguage/");
            stringBuilder.AppendLine("disallow: /webMenu/");
            stringBuilder.AppendLine("disallow: /webNews/");
            stringBuilder.AppendLine("disallow: /webNewsTypes/");
            stringBuilder.AppendLine("disallow: /webPageMaintenance/");
            stringBuilder.AppendLine("disallow: /webStaff/");
            stringBuilder.AppendLine("disallow: /webStory/");

            stringBuilder.AppendLine("allow: /");
            stringBuilder.Append("sitemap: ");
            stringBuilder.AppendLine(this.Url.RouteUrl("GetSitemapXml", null, this.Request.Url.Scheme).TrimEnd('/'));

            return this.Content(stringBuilder.ToString(), "text/plain", Encoding.UTF8);
        }
        #endregion
        private long getLanguageId()
        {
            var languageId = 1;
            if (Request.Cookies["Company"] != null)
            {
                languageId = int.Parse(Request.Cookies["Company"].Values["languageId"].ToString().Trim());
            }
            if (!_context.Get<WebLanguage>(d => d.Id == languageId).Any())
            {
                languageId = 1;
            }
            return languageId;
        }
        public ActionResult Loading()
        {
            return View();
        }

        [HttpPost]
        public JsonResult _Contact(ContactViewModel form)
        {
            var languageId = getLanguageId();
            var Email=_context.Get<College>(d=>d.LanguageId==languageId).FirstOrDefault().Email;
            int colon = Email.IndexOf(":");
            if (colon != -1)
            {
                Email = Email.Substring(colon+1);
            }
            colon = Email.IndexOf("：");
            if (colon != -1)
            {
                Email = Email.Substring(colon+1);
            }

            var emailReturn = EmailWithLink.Send(Email,
                       form.Email,
                       DateTime.Today.ToString("dd-MMM-yyyy") + " "+ form.Name +" ask for informatiom",
                       "Name:"+form.Name+"<br> Email address:"+form.Email+"<Br> Phone number:"+form.Phone+"<br> Detail:"+form.Content);
            if (emailReturn < 0)
            {
                return Json("failed", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("seccessful", JsonRequestBehavior.AllowGet);
            }
        }
    }
}