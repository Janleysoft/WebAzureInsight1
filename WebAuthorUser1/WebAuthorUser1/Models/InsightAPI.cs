using System;
using System.Collections.Generic;
using System.Configuration;
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
        //send alert email :warning
        public static string DurationEmail(string appName, int totalDuration)
        {
            MailMessage m = new MailMessage();
            SmtpClient sc = new SmtpClient();
            string emailStatus = "Send an Email successfully. Warning!";
            string bodyMessage = null;
            var password= ConfigurationManager.AppSettings["MyPassword"];
            var address= ConfigurationManager.AppSettings["gmailAddress"];
            try
            {
                bodyMessage = "Hi Peter, <br><br>"
                                            + "Warning! The request which the total processing duration over 800ms: <br><br>"
                                            + "appName:" + appName + " <br><br>"
                                            + "total processing Duration: " + totalDuration + " <br><br>"
                                            + "Best regards,<br><br>"
                                            + "Janley";
                //send Email
                m.From = new MailAddress(address, "Janley Zhang");
                MailAddress to = new MailAddress("janleysoft@outlook.com", "Janley Zhang");
                m.To.Add(to);
                //similarly BCC
                m.Subject = "The request Test from Application Insight"; m.IsBodyHtml = true; m.Body = bodyMessage;
                sc.Host = "smtp.gmail.com";
                sc.Port = 587;
                sc.UseDefaultCredentials = false;
                sc.Credentials = new System.Net.NetworkCredential(address, password);
                sc.EnableSsl = true;
                sc.Send(m);              
            }
            catch (Exception ex)
            {
                emailStatus = ex.Message;
            }
            return emailStatus;
        }

        public static string SuccessEmail(string appName, int totalDuration)
        {
            string password = ConfigurationManager.AppSettings["MyPassword"];
            MailMessage m = new MailMessage();
            SmtpClient sc = new SmtpClient();
            string emailStatus = "Send an Email successfully. Succeed!";
            string bodyMessage = null;
            try
            {
                bodyMessage = "Hi Peter, <br><br>"
                                            + "Succeed! The request which the total processing duration less 1000ms: <br><br>"
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
                sc.Credentials = new System.Net.NetworkCredential("janleysoft@gmail.com", password);
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