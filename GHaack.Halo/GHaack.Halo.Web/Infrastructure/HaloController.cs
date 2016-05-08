using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using Microsoft.Web.Mvc;

namespace GHaack.Halo.Web.Infrastructure
{
    public abstract class HaloController : Controller
    {
        protected ActionResult RedirectToAction<TController>(Expression<Action<TController>> action)
            where TController : Controller
        {
            return ControllerExtensions.RedirectToAction(this, action);
        }
    }
}