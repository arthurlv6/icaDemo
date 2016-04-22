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
    public class WebLanguageController : AppFrameController
    {
        // GET: Alumni
        private readonly IBaseOperations _context;
        private readonly ICurrentUser _currentUser;
        public WebLanguageController(IBaseOperations context,ICurrentUser currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }
        public ActionResult Index()
        {
            return View(_context.Get<WebLanguage>(d=>true));
        }
        [HttpPost]
        public ActionResult Index(FormCollection input)
        {
            var IdArray = input.GetValues("item.Id");
            var NameArray = input.GetValues("item.Name");
            int rzt = 0;
            if (rzt < 0)
                return Json("failed", JsonRequestBehavior.AllowGet);
            return Json("done", JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Add(WebLanguage input)
        {
            #region create new data
            var newLanguage = new WebLanguage { Name = input.Name };
            //menu
            foreach (var level1 in _context.Get<WebMenu>(d => d.LanguageId == 1 && d.ParentId == null).ToList())
            {
                var Newlevel1 = new WebMenu() { Name = level1.Name, Content = level1.Content,Link=level1.Link,Order=level1.Order };
                foreach (var level2 in _context.Get<WebMenu>(d => d.LanguageId == 1 && d.ParentId == level1.Id).ToList())
                {
                    var Newlevel2 = new WebMenu() { Name = level2.Name, Content = level2.Content, Link = level2.Link, Order = level2.Order };
                    foreach (var level3 in _context.Get<WebMenu>(d => d.LanguageId == 1 && d.ParentId == level2.Id).ToList())
                    {
                        Newlevel2.WebMenuChildren.Add(new WebMenu() { Name = level3.Name, Content = level3.Content, Link = level3.Link, Order = level3.Order });
                    }
                    Newlevel1.WebMenuChildren.Add(Newlevel2);
                }
                newLanguage.WebMenus.Add(Newlevel1);
            }
            //college
            var college = _context.FindDetail<College>(1);
            _context.EntityCloneNew(college);
            newLanguage.College = college;

            //carousel
            foreach (var item in _context.Get<WebCarousel>(d => d.LanguageId == 1).ToList())
            {
                _context.EntityCloneNew(item);
                newLanguage.WebCarousels.Add(item);
            }
            //newsType
            foreach (var item in _context.Get<WebNewsType>(d => d.LanguageId == 1).Include(d=>d.WebNews).ToList())
            {
                _context.EntityCloneNew(item);
                foreach (var news in item.WebNews)
                {
                    _context.EntityCloneNew(news);
                }
                newLanguage.WebNewsTypes.Add(item);
            }
            //events
            foreach (var item in _context.Get<WebEvent>(d => d.LanguageId == 1).ToList())
            {
                _context.EntityCloneNew(item);
                newLanguage.WebEvents.Add(item);
            }
            //JobType
            foreach (var item in _context.Get<WebJobType>(d => d.LanguageId == 1).ToList())
            {
                _context.EntityCloneNew(item);
                newLanguage.WebJobTypes.Add(item);
            }
            //Departments
            foreach (var item in _context.Get<WebDepartment>(d => d.LanguageId == 1).Include(d=>d.WebStaff).Include(d=>d.WebStories).Include(d=>d.WebStudents).ToList())
            {
                _context.EntityCloneNew(item);
                foreach (var staff in item.WebStaff)
                {
                    _context.EntityCloneNew(staff);
                }
                foreach (var story in item.WebStories)
                {
                    _context.EntityCloneNew(story);
                }
                foreach (var student in item.WebStudents)
                {
                    _context.EntityCloneNew(student);
                }
                newLanguage.WebDepartment.Add(item);
            }
            //userful link
            foreach (var item in _context.Get<WebUsefulLink>(d => d.LanguageId == 1).ToList())
            {
                _context.EntityCloneNew(item);
                newLanguage.WebUsefulLinks.Add(item);
            }
            _context.AddOneEntity(newLanguage);
            #endregion
            #region update languageId
            //newsType
            var languageId = newLanguage.Id;
            foreach (var item in _context.Get<WebNewsType>(d => d.LanguageId == languageId).Include(d => d.WebNews).ToList())
            {
                foreach (var news in item.WebNews)
                {
                    news.LanguageId = languageId;
                }
            }
            //Departments
            foreach (var item in _context.Get<WebDepartment>(d => d.LanguageId == languageId).Include(d => d.WebStaff).Include(d => d.WebStories).Include(d => d.WebStudents).ToList())
            {
                foreach (var staff in item.WebStaff)
                {
                    staff.LanguageId = languageId;
                }
                foreach (var story in item.WebStories)
                {
                    story.LanguageId = languageId;
                }
                //foreach (var student in item.WebStudents)
                //{
                    
                //}
            }
            _context.SaveChange();
            #endregion
            return RedirectToAction<WebLanguageController>(d=>d.Index());
        }
    }
}