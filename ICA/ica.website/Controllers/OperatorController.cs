using AutoMapper;
using AutoMapper.QueryableExtensions;
using ica.website.Filters;
using ica.website.Infrastructure;
using ica.website.Infrastructure.Alerts;
using ica.website.Models;
using ica.website.Models.Alumni;
using ica.website.Models.Operator;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace ica.website.Controllers
{
    [Authorize(Roles = "Operators Management")]
    public class OperatorController : AppFrameController
    {
        // GET: Operators
        private readonly IBaseOperations _context;
        private readonly ICurrentUser _currentUser;
        public OperatorController(IBaseOperations context,ICurrentUser currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }
        public ActionResult Index()
        {
            //ApplicationUser tem = new ApplicationUser();
            var models = _context.GetSearchResult(new ConditionViewModel<ApplicationUser>()
            {
                Func=t=>t.PasswordHash!=null,
                ChangeOrderDirection = false,
                OrderDirection = "desc"
            }, d => d.UpdateDatetime, new string[] {"Roles"}, true);
            var target = Mapper.Map<ConditionViewModelTarget<OperatorViewModel>>(models);
            var temp = models.Data.Project().To<OperatorViewModel>().ToList();
            foreach (var item in temp)
            {
                item.IdForJS = item.Id.Replace("-", "");
            }
            target.Data = temp;
            return View(target);
        }
        [HttpPost]
        public ActionResult Index(ConditionViewModelTarget<OperatorViewModel> input)
        {
            var inputForSearch = Mapper.Map<ConditionViewModel<ApplicationUser>>(input);
            Expression<Func<ApplicationUser, bool>> searchName;
            if (string.IsNullOrEmpty(input.Search))
            {
                searchName = t => t.PasswordHash!=null;
            }
            else
            {
                searchName = t => t.PasswordHash != null && (t.UserName.Contains(input.Search) || t.Company.Contains(input.Search) || t.StudentId.Contains(input.Search) || t.JobTitle.Contains(input.Search));
            }
            inputForSearch.Func = searchName;
            var includes = new string[] { "Roles" };
            switch (input.Order)
            {
                case "UserName":
                    inputForSearch=_context.GetSearchResult(inputForSearch, d => d.UserName, includes, false);
                    break;
                case "Department":
                    inputForSearch = _context.GetSearchResult(inputForSearch, d => d.Department, includes, false);
                    break;
                case "JobStatus":
                    inputForSearch = _context.GetSearchResult(inputForSearch, d => d.JobStatus, includes, false);
                    break;
                case "Position":
                    inputForSearch = _context.GetSearchResult(inputForSearch, d => d.JobTitle, includes, false);
                    break;
                case "Phone":
                    inputForSearch = _context.GetSearchResult(inputForSearch, d => d.PhoneNumber, includes, false);
                    break;
                default:
                    inputForSearch = _context.GetSearchResult(inputForSearch, d => d.UpdateDatetime, includes, false);
                    break;
            }
            var target = Mapper.Map<ConditionViewModelTarget<OperatorViewModel>>(inputForSearch);
            var temp = inputForSearch.Data.Project().To<OperatorViewModel>().ToList();
            foreach (var item in temp)
            {
                item.IdForJS = item.Id.Replace("-", "");
            }
            target.Data = temp;
            return View(target);
        }
      
        [HttpPost]
        public JsonResult _OperatorEdit(OperatorViewModel input)
        {
            _context.Get<ApplicationUser>(d => d.Id == input.Id).FirstOrDefault().UpdateDatetime = _context.GetNzTime();
            var opteratorRoles=_context.Get<IdentityUserRole>(d => d.UserId == input.Id).ToList();
            foreach (var item in opteratorRoles)
            {
                if (input.RolePosted != null)
                {
                    if (!input.RolePosted.ids.Any(d => d == item.RoleId))
                    {
                        var entity=_context.Get<IdentityUserRole>(d=>d.RoleId==item.RoleId && d.UserId==input.Id).FirstOrDefault();
                        _context.GetTranscaiton().Entry(entity).State = EntityState.Deleted;
                    }
                }
                else
                {
                    var entity = _context.Get<IdentityUserRole>(d => d.RoleId == item.RoleId && d.UserId == input.Id).FirstOrDefault();
                    _context.GetTranscaiton().Entry(entity).State = EntityState.Deleted;
                }
            }
            _context.SaveChange();
            if (input.RolePosted != null)
            {
                foreach (var item in input.RolePosted.ids)
                {
                    if (!opteratorRoles.Any(d => d.RoleId == item))
                    {
                        _context.AddOneEntity<IdentityUserRole>(new IdentityUserRole { UserId = input.Id, RoleId = item });
                    }
                }
            }
            return Json("1", JsonRequestBehavior.AllowGet);
        }
    }
}