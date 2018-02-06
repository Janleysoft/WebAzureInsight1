using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace WebJobEmail1
{
        //add CRON https://blogs.msdn.microsoft.com/benjaminperkins/2016/09/01/how-to-setup-cron-to-run-a-webjob-on-the-azure-app-service-platform/
        public class InsightAPI
        {
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

            //send alert email :warning
            public static string WarnEmail(double cpuAvg)
            {
                MailMessage m = new MailMessage();
                SmtpClient sc = new SmtpClient();
                string emailStatus = "Send an Email successfully. Warning!";
                string bodyMessage = null;
                var password = ConfigurationManager.AppSettings["MyPassword"];
                var address = ConfigurationManager.AppSettings["gmailAddress"];
                try
                {
                    bodyMessage = "Hi Peter, <br><br>"
                                                + "Warning! The process cpu percentage over 0.10: <br><br>"

                                                + "Process CPU percentage: " + cpuAvg + " <br><br>"
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

            public static string SuccessEmail(double cpuAvg)
            {
                string password = ConfigurationManager.AppSettings["MyPassword"];
                MailMessage m = new MailMessage();
                SmtpClient sc = new SmtpClient();
                string emailStatus = "Send an Email successfully. Succeed!";
                string bodyMessage = null;
                try
                {
                    bodyMessage = "Hi Peter, <br><br>"
                                                + "Succeed! The process cpu percentage less 0.10: <br><br>"

                                                + "total processing Duration: " + cpuAvg + " <br><br>"
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
