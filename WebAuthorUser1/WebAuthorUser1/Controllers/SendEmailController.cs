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
        // GET: SendEmail
        public ActionResult Index()
        {
            MailMessage m = new MailMessage();
            SmtpClient sc = new SmtpClient();
            string emailStatus = "Send an Email:";
            try
            {

                ApplicationDbContext db = new ApplicationDbContext();
                int failCount = db.QueryMVC16s.Where(p => p.success=="false").Count();
                var failrequest = db.QueryMVC16s.Where(p => p.success == "false").FirstOrDefault();
                var successrequest = db.QueryMVC16s.Where(p => p.success == "true").FirstOrDefault();
                string requestStatus = "success";
                string bodyMessage = null;
                string owner = "Peter";
                if (failCount >= 3)
                {
                    requestStatus = "failed";
                     bodyMessage = "Hi " + owner + ",<br><br>"
                                            + "The request status from Application Insight: <br><br>"
                                            + requestStatus + " <br><br>"
                                            + "name: " + failrequest.name + " <br><br>"
                                            + "url: " + failrequest.url + " <br><br>"
                                            + "resultCode: " + failrequest.resultCode + " <br><br>"
                                            + "appName: " + failrequest.appName + " <br><br>"
                                            + "Best regards,<br><br>"
                                            + "Janley";
                }
                else
                {
                    requestStatus = "success";
                    bodyMessage = "Hi " + owner + ",<br><br>"
                                           + "The request status from Application Insight: <br><br>"
                                           + requestStatus + " <br><br>"
                                           + "name: " + successrequest.name + " <br><br>"
                                           + "url: " + successrequest.url + " <br><br>"
                                           + "resultCode: " + successrequest.resultCode + " <br><br>"
                                           + "appName: " + successrequest.appName + " <br><br>"
                                           + "Best regards,<br><br>"
                                           + "Janley";
                }
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
            ViewBag.Status = emailStatus;
            return View();
}
    }
}