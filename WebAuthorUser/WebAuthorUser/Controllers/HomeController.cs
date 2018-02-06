using Microsoft.ApplicationInsights.DataContracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAuthorUser.Models;

namespace WebAuthorUser.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Trace.TraceInformation("my trace info Home/Index");
            var telemetry = new Microsoft.ApplicationInsights.TelemetryClient();
            telemetry.TrackTrace("Home/Index Main");
            telemetry.TrackPageView("Home/Index");
            return View();
        }
        public ActionResult Test()
        {
            var telemetry = new Microsoft.ApplicationInsights.TelemetryClient();
            telemetry.Context.Operation.Name = "MyOperationName1";
            telemetry.TrackTrace("database response",
               SeverityLevel.Warning,
               new Dictionary<string, string> { { "database", "myName" } });
            return View();
        }

        public ActionResult About()
        {
            Trace.TraceInformation("my trace info Home/About");
            ViewBag.Message = "Your application description page.";
            ViewBag.status = "About Exception";
            var r = new Random();
            if (r.Next() % 3 == 0)
            {
                // Trace.TraceInformation("Home/About Error");
                ViewBag.status = "get error";
                var c = new AboutModels();
                c.saySomething();
            }
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            ViewBag.status = "Contact Exception";
            var r = new Random();
            if (r.Next() % 2 == 0)
            {
                ViewBag.status = "get error";
                var c = new ContactModels();
                c.doSomething();
            }
            return View();
        }
    }
}