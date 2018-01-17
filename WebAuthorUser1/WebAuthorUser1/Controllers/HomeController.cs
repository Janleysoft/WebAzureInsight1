using Microsoft.ApplicationInsights.DataContracts;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using WebAuthorUser1.Models;

namespace WebAuthorUser1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var telemetry = new Microsoft.ApplicationInsights.TelemetryClient();
            telemetry.TrackTrace("Home/Index Main");
            return View();
        }
        public ActionResult Test()
        {
            var telemetry = new Microsoft.ApplicationInsights.TelemetryClient();
            telemetry.Context.Operation.Name = "MyOperationName1";
            var dependency = new DependencyTelemetry();
            var startTime = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();
            timer.Stop();
            TimeSpan ts = new TimeSpan(0,0,5);
                        telemetry.TrackDependency("testDependency", "baseTest1", "testDependency", "remoteTest1", startTime, ts, "201", false);          
            return View();
        }
        [Authorize]
        public ActionResult ExcelTest()
        {
           
            return View();
        }
        [HttpPost]
        public ActionResult ExcelTest(HttpPostedFileBase postedFile)
        {
            string filePath = string.Empty;
            if (postedFile != null)
            {
                 string path = Server.MapPath("~/Uploads/");               
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                filePath = path + Path.GetFileName(postedFile.FileName);
                string extension = Path.GetExtension(postedFile.FileName);
                postedFile.SaveAs(filePath);

                string conString = string.Empty;
                switch (extension)
                {
                    case ".xls": //Excel 97-03.
                        conString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                        break;
                    case ".xlsx": //Excel 07 and above.
                        conString = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                        break;
                }

                DataTable dt = new DataTable();
                conString = string.Format(conString, filePath);

                using (OleDbConnection connExcel = new OleDbConnection(conString))
                {
                    using (OleDbCommand cmdExcel = new OleDbCommand())
                    {
                        using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                        {
                            cmdExcel.Connection = connExcel;

                            //Get the name of First Sheet.
                            connExcel.Open();
                            DataTable dtExcelSchema;
                            dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                            string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                            connExcel.Close();

                            //Read Data from First Sheet.
                            connExcel.Open();
                            cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                            odaExcel.SelectCommand = cmdExcel;
                            odaExcel.Fill(dt);
                            connExcel.Close();
                        }
                    }
                }

                conString = ConfigurationManager.ConnectionStrings["ConnectionStringName"].ConnectionString;
                using (SqlConnection con = new SqlConnection(conString))
                {
                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                    {
                        //con.Open();
                        //Set the database table name.
                        sqlBulkCopy.DestinationTableName = "dbo.QueryMVC16";
                        //[OPTIONAL]: Map the Excel columns with that of the database table
                        sqlBulkCopy.ColumnMappings.Add("timestamp", "timestamp");
                        sqlBulkCopy.ColumnMappings.Add("name", "name");
                        sqlBulkCopy.ColumnMappings.Add("url", "url");
                        sqlBulkCopy.ColumnMappings.Add("success", "success");
                        sqlBulkCopy.ColumnMappings.Add("resultCode", "resultCode");
                        sqlBulkCopy.ColumnMappings.Add("operation_Name", "operation_Name");
                        sqlBulkCopy.ColumnMappings.Add("cloud_RoleInstance", "cloud_RoleInstance");
                        sqlBulkCopy.ColumnMappings.Add("appName", "appName");
                        con.Open();
                        sqlBulkCopy.WriteToServer(dt);
                        con.Close();
                    }
                }
            }

            return View();
        }   
    public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            var r = new Random();
            if (r.Next() % 3 == 0)
            {
                Trace.TraceInformation("Home/About Error");
                var c = new AboutModels();
                c.saySomething();
            }
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            var r = new Random();
            if (r.Next() % 2 == 0)
            {             
                var c = new ContactModels();
                c.doSomething();
            }
            return View();
        }
    }
}