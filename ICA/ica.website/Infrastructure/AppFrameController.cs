using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using Microsoft.Web.Mvc;
using ica.website.Filters;

namespace ica.website.Infrastructure
{
    public abstract class AppFrameController : Controller
    {
        protected ActionResult RedirectToAction<TController>(
            Expression<Action<TController>> action)
            where TController : Controller
        {
            return ControllerExtensions.RedirectToAction(this, action);
        }

    }
}