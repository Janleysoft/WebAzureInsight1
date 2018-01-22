using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using WebAuthorUser1.Models;

namespace WebAuthorUser1.Controllers
{
    public class SendEmailController : Controller
    {
       // private const string URL =
      // "https://api.applicationinsights.io/v1/apps/d52f97b1-9ac2-4558-89ef-331f5fc2aac9/events/browserTimings?$top=5";
        // GET: SendEmail
        public ActionResult Index()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var count=db.InsightEventDatas.Count();
            if (count > 0)
            {
                db.Database.ExecuteSqlCommand("TRUNCATE TABLE InsightEventDatas");
            }
            var json =InsightAPI.GetTelemetry
                ("d52f97b1-9ac2-4558-89ef-331f5fc2aac9", "wmjvrwg8cbq6r5rj2ngoch3r2artrvt8ukj1np61", "interval=PT12H");
            var result = JObject.Parse(json);
            var a = (int)result["value"].Count(); //request count
            Console.WriteLine("result number:" + a);
            int totalProcessingDuration = 0;
            var appName = (string)result["value"][0]["ai"]["appName"];
            for (int i = 0; i < a; i++)
            {
                var timestamp = (DateTime)result["value"][i]["timestamp"];
                var url = (string)result["value"][i]["browserTiming"]["url"];
                var processingDuration = (int)result["value"][i]["browserTiming"]["processingDuration"];
                Console.WriteLine("appName:" + appName + ", url: " + url + ", processingDuration:" + processingDuration);
                totalProcessingDuration += processingDuration;
                db.InsightEventDatas.Add(new InsightEventData() { url = url, appName = appName, processingDuration = processingDuration,MyTimeStamp=timestamp });
            }
            db.SaveChanges();
            if (totalProcessingDuration > 1000)
            {
               string emailStatus= InsightAPI.DurationEmail(appName, totalProcessingDuration);
                ViewBag.emailStatus = emailStatus;
            }
            return View();
}
    }
}