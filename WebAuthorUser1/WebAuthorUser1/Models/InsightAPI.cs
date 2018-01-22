using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Web;

namespace WebAuthorUser1.Models
{
    public class InsightAPI
    {
        private const string URL =
      "https://api.applicationinsights.io/v1/apps/d52f97b1-9ac2-4558-89ef-331f5fc2aac9/events/browserTimings?$top=5";

        //get telemetry data by using Application Insight Rest API
        //url:https://dev.applicationinsights.io/apiexplorer/events?appId=d52f97b1-9ac2-4558-89ef-331f5fc2aac9&apiKey=wmjvrwg8cbq6r5rj2ngoch3r2artrvt8ukj1np61&eventType=browserTimings
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
        //send email 
        public static string DurationEmail(string appName, int totalDuration)
        {
            MailMessage m = new MailMessage();
            SmtpClient sc = new SmtpClient();
            string emailStatus = "Send an Email successfully.";
            string bodyMessage = null;
            try
            {
                bodyMessage = "Hi Peter, <br><br>"
                                            + "The request which the total processing duration over 800ms: <br><br>"
                                            + "appName:" + appName + " <br><br>"
                                            + "total processing Duration: " + totalDuration + " <br><br>"
                                            + "Best regards,<br><br>"
                                            + "Janley";
                //send Email
                m.From = new MailAddress("janleysoft@gmail.com", "Janley Zhang");
                MailAddress to = new MailAddress("janleysoft@outlook.com", "Janley Zhang");
                m.To.Add(to);
                //similarly BCC
                m.Subject = "The request Test from Application Insight"; m.IsBodyHtml = true; m.Body = bodyMessage;
                sc.Host = "smtp.gmail.com";
                sc.Port = 587;
                sc.UseDefaultCredentials = false;
                sc.Credentials = new System.Net.NetworkCredential("janleysoft@gmail.com", "031351203636TF");
                sc.EnableSsl = true;
                sc.Send(m);              
            }
            catch (Exception ex)
            {
                emailStatus = ex.Message;
            }
            return emailStatus;
        }

    }
}