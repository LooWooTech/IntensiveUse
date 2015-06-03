using IntensiveUse.Manager;
using IntensiveUse.Helper;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace IntensiveUse.Controllers
{
    public class ControllerBase : AsyncController
    {
        protected ManagerCore Core = new ManagerCore();
        private Exception GetException(Exception ex)
        {
            var innerEx = ex.InnerException;
            if (innerEx != null)
            {
                return GetException(innerEx);
            }
            return ex;
        }

        protected ActionResult JsonFail(string Message)
        {
            return Content(new { result = false, Message }.ToJson());
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled) return;
            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.StatusCode = 500;
            ViewBag.Exception = GetException(filterContext.Exception);
            filterContext.Result = View("Error");
        }

        protected ActionResult HtmlResult(List<string> html)
        {
            string str = string.Empty;
            foreach (var item in html)
            {
                str += "<option value='" + item + "'>" + item + "</option>";
            }
            return Content(str);
        }

    }
}
