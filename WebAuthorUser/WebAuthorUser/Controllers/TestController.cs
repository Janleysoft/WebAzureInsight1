using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

namespace WebAuthorUser.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Index()
        {
            var json =GetTelemetry
               ("a38047dd-f0a9-47d4-b398-e8c57dec9ed3", "83e6cxyav0bxwzvglxkqxn46bxcypb760d66c51v", "");
            var result = JObject.Parse(json);
            var startTime = (string)result["value"]["start"];
            ViewBag.startTime = startTime;
            var endTime = (string)result["value"]["end"];
            ViewBag.endTime = endTime;
            var cpuAvg = (double)result["value"]["performanceCounters/processCpuPercentage"]["avg"];
            ViewBag.cpuAvg = cpuAvg;
            return View();
        }
        private const string URL =
          "https://api.applicationinsights.io/v1/apps/a38047dd-f0a9-47d4-b398-e8c57dec9ed3/metrics/performanceCounters/processCpuPercentage?timespan=P7D&aggregation=avg";

        public static string GetTelemetry(string appid, string apikey, string parameterString)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("x-api-key", apikey);
            var req = string.Format(URL, appid, parameterString);
            HttpResponseMessage response = client.GetAsync(req).Result;
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsStringAsync().Result;
            }
            else
            {
                return response.ReasonPhrase;
            }
        }
    }
}