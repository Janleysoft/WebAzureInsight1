using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebAuthorUser1.Models;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using WebAuthorUser1.Processor;

namespace WebAuthorUser1.Controllers
{
    public class StudentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Students
        public ActionResult Index()
        {
            //Application Insight Tracing log:https://docs.microsoft.com/en-us/azure/application-insights/app-insights-asp-net-trace-logs
            Trace.TraceInformation("Students/Index Get");
            TelemetryClient telemetry = new TelemetryClient();
            //Event
            telemetry.TrackEvent("StudentIndex");

            // Establish an operation context and associated telemetry item:
            using (var operation = telemetry.StartOperation<RequestTelemetry>("GET Students/Index"))
            {
    // Telemetry sent in here will use the same operation ID.
    telemetry.TrackTrace("Trace Index Operation"); // or other Track* calls
    // Set properties of containing telemetry item--for example:
    operation.Telemetry.ResponseCode = "200";
                // Optional: explicitly send telemetry item:
                telemetry.StopOperation(operation);
            
            } // When operation is disposed, telemetry item is sent.
              
            //custom metric
            var sample = new MetricTelemetry();
            sample.Name = "metricname2";
            sample.Sum = 22.1;
            telemetry.TrackMetric(sample);
            //Dependency
            var success = false;
            var startTime = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();
            TraceTelemetry aa = new TraceTelemetry();
            //Request
            telemetry.TrackRequest("Custom Request", startTime, timer.Elapsed, "200", success);
            // var startTime2 = DateTime.Now.AddSeconds(6);
            timer.Stop();   
            telemetry.TrackDependency("myDependency", "baseName1", "myDependency", "remoteName1", startTime, timer.Elapsed,"201",success);           
            return View(db.Students.ToList());
        }

        // GET: Students/Details/5
        public ActionResult Details(int? id)
        {
            Trace.TraceInformation("Students/Details Get ");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }       
        // GET: Students/Create
        public ActionResult Create()
        {
            Trace.TraceInformation("Students/Create Get");
            return View();
        }

       [Authorize(Roles ="janleyzhang")]
        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Age,Address")] Student student)
        {
            Trace.TraceInformation("Students/Create Post");
            if (ModelState.IsValid)
            {
                db.Students.Add(student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(student);
        }

        // GET: Students/Edit/5   
        [Authorize(Roles = "janleyzhang")]
        public ActionResult Edit(int? id)
        {
            Trace.TraceInformation("Students/Edit Get");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Age,Address")] Student student)
        {
            Trace.TraceInformation("Students/Edit Post");
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(student);
        }

        // GET: Students/Delete/5
        [Authorize(Roles = "janleyzhang")]
        public ActionResult Delete(int? id)
        {
            Trace.TraceInformation("Students/Delete Get");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Delete/5
        [Authorize(Roles ="janleyzhang")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Trace.TraceInformation("Students/Delete Post");
            Student student = db.Students.Find(id);
            db.Students.Remove(student);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
