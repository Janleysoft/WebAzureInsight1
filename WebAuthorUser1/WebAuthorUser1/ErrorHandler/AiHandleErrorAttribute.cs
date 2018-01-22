using Microsoft.ApplicationInsights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAuthorUser1.ErrorHandler;

namespace WebAuthorUser1.ErrorHandler
{
    //public class AiHandleErrorAttribute : HandleErrorAttribute
    //{
    //    public override void OnException(ExceptionContext filterContext)
    //    {
    //        if (filterContext != null && filterContext.HttpContext != null && filterContext.Exception != null)
    //        {
    //            //If customError is Off, then AI HTTPModule will report the exception
    //            //If it is On, or RemoteOnly (default) - then we need to explicitly track the exception
    //            if (filterContext.HttpContext.IsCustomErrorEnabled)
    //            {
    //                var ai = new TelemetryClient();
    //                ai.TrackException(filterContext.Exception);
    //            }
    //        }
    //        base.OnException(filterContext);
    //    }
    //}
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class AiHandleErrorAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext != null && filterContext.HttpContext != null && filterContext.Exception != null)
            {
                //If customError is Off, then AI HTTPModule will report the exception
                if (filterContext.HttpContext.IsCustomErrorEnabled)
                {
                    // Note: A single instance of telemetry client is sufficient to track multiple telemetry items.
                    var ai = new TelemetryClient();
                    ai.TrackException(filterContext.Exception);
                }
            }
            base.OnException(filterContext);
        }
    }
}
// then register AiHandleErrorAttribute in FilterConfig:
public class FilterConfig
{
    public static void RegisterGlobalFilters(GlobalFilterCollection filters)
    {
        filters.Add(new AiHandleErrorAttribute());
    }
}