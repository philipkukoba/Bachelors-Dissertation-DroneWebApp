using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using DroneWebApp.Models;
using DroneWebApp.Models.Helper;

namespace DroneWebApp.Controllers
{
    public class DroneFlightsController : Controller
    {
        private DroneDBEntities db;

        //Constructor
        public DroneFlightsController(DbContext db)
        {
            this.db = (DroneDBEntities) db;
        }

        // GET: DroneFlights
        public ActionResult Index()
        {
            var droneFlights = db.DroneFlights;
            //var droneFlights = db.DroneFlights.Include(d => d.Drone).Include(d => d.Pilot).Include(d => d.Project);
            return View(droneFlights.ToList());
        }

        // GET: DroneFlights/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                ViewBag.ErrorMessage = "Please specify a Drone Flight in your URL.";
                return View("~/Views/ErrorPage/Error.cshtml");
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DroneFlight droneFlight = db.DroneFlights.Find(id);
            if (droneFlight == null)
            {
                ViewBag.ErrorMessage = "Drone Flight could not be found.";
                return View("~/Views/ErrorPage/Error.cshtml");
                //return HttpNotFound();
            }
            return View(droneFlight);
        }

        // GET: DroneFlights/Create
        public ActionResult Create(int? pilotId, int? projectId)
        {
            if(pilotId != null)
            {
                ViewBag.PilotId = new SelectList(db.Pilots, "PilotId", "PilotName", pilotId);
            }
            else
            {
                ViewBag.PilotId = new SelectList(db.Pilots, "PilotId", "PilotName");
            }
            if(projectId != null)
            {
                ViewBag.ProjectId = new SelectList(db.Projects, "ProjectId", "ProjectCode", projectId);
            }
            else
            {
                ViewBag.ProjectId = new SelectList(db.Projects, "ProjectId", "ProjectCode");
            }
            ViewBag.DroneId = new SelectList(db.Drones, "DroneId", "DroneName");
            return View();
        }

        // POST: DroneFlights/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FlightId, DroneId, PilotId, ProjectId, Location, Date, TypeOfActivity, Other, Simulator, Instructor, Remarks, hasTFW, hasGCPs, hasCTRLs, hasDepInfo, hasDestInfo, hasQR, hasXYZ, hasDroneLog")] DroneFlight droneFlight)
        {
            if (ModelState.IsValid)
            {
                db.DroneFlights.Add(droneFlight);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProjectId = new SelectList(db.Projects, "ProjectId", "ProjectCode", droneFlight.ProjectId);
            ViewBag.DroneId = new SelectList(db.Drones, "DroneId", "DroneName", droneFlight.DroneId);
            ViewBag.PilotId = new SelectList(db.Pilots, "PilotId", "PilotName", droneFlight.PilotId);
            return View(droneFlight);
        }

        // GET: DroneFlights/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                ViewBag.ErrorMessage = "Please specify a Drone Flight in your URL.";
                return View("~/Views/ErrorPage/Error.cshtml");
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DroneFlight droneFlight = db.DroneFlights.Find(id);
            if (droneFlight == null)
            {
                ViewBag.ErrorMessage = "Drone Flight could not be found.";
                return View("~/Views/ErrorPage/Error.cshtml");
                //return HttpNotFound();
            }
            ViewBag.ProjectId = new SelectList(db.Projects, "ProjectId", "ProjectCode", droneFlight.ProjectId);
            ViewBag.DroneId = new SelectList(db.Drones, "DroneId", "DroneName", droneFlight.DroneId);
            ViewBag.PilotId = new SelectList(db.Pilots, "PilotId", "PilotName", droneFlight.PilotId);
            return View(droneFlight);
        }

        // POST: DroneFlights/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FlightId, DroneId, PilotId, ProjectId, Location, Date, TypeOfActivity, Other, Simulator, Instructor, Remarks")] DroneFlight droneFlight)
        {
            if (ModelState.IsValid)
            {
                DroneFlight df = db.DroneFlights.Find(droneFlight.FlightId);
                UpdateFlightFields(droneFlight, df);
                System.Diagnostics.Debug.WriteLine("In modelstate is valid, before modified");
                db.Entry(df).State = EntityState.Modified;
                db.SaveChanges();
                // Update the total time drones have flown in case the drone flight's drone has been changed by the user
                Helper.UpdateTotalDroneFlightTime(this.db);
                return RedirectToAction("Index");
            }
            ViewBag.ProjectId = new SelectList(db.Projects, "ProjectId", "ProjectCode", droneFlight.ProjectId);
            ViewBag.DroneId = new SelectList(db.Drones, "DroneId", "DroneName", droneFlight.DroneId);
            ViewBag.PilotId = new SelectList(db.Pilots, "PilotId", "PilotName", droneFlight.PilotId);
            return View(droneFlight);
        }

        // Update the fields of the DroneFlight that has been found by FlightId with the fields of the posted DroneFlight, a.k.a. the drone flight 
        // the user has submitted
        private static void UpdateFlightFields(DroneFlight postedDroneFlight, DroneFlight df)
        {
            df.DroneId = postedDroneFlight.DroneId;
            df.PilotId = postedDroneFlight.PilotId;
            df.ProjectId = postedDroneFlight.ProjectId;
            df.Location = postedDroneFlight.Location;
            df.Date = postedDroneFlight.Date;
            df.TypeOfActivity = postedDroneFlight.TypeOfActivity;
            df.Other = postedDroneFlight.Other;
            df.Simulator = postedDroneFlight.Simulator;
            df.Instructor = postedDroneFlight.Instructor;
            df.Remarks = postedDroneFlight.Remarks;
        }

        // GET: DroneFlights/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                ViewBag.ErrorMessage = "Please specify a Drone Flight in your URL.";
                return View("~/Views/ErrorPage/Error.cshtml");
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DroneFlight droneFlight = db.DroneFlights.Find(id);
            if (droneFlight == null)
            {
                ViewBag.ErrorMessage = "Drone Flight could not be found.";
                return View("~/Views/ErrorPage/Error.cshtml");
                //return HttpNotFound();
            }
            return View(droneFlight);
        }

        // POST: DroneFlights/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            DroneFlight droneFlight = db.DroneFlights.Find(id);
            try
            {
                db.DroneFlights.Remove(droneFlight);
                db.SaveChanges();
            }
            catch(Exception)
            {
                ViewBag.ErrorDroneFlightDelete = "Cannot delete this Drone Flight.";
                return View(droneFlight);
            }
            // Update the total time drones have flown in case the drone flight's drone has been changed by the user
            Helper.UpdateTotalDroneFlightTime(this.db);
            return RedirectToAction("Index");
        }

        public ActionResult QualityReport(int? id)
        {
            if (id == null)
            {
                ViewBag.ErrorMessage = "Please specify a Drone Flight in your URL.";
                return View("~/Views/ErrorPage/Error.cshtml");
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // Get the appropriate drone flight per id
            DroneFlight droneFlight = db.DroneFlights.Find(id);
            // Get the drone flight's Quality Report
            QualityReport qualityReport = droneFlight.QualityReport;
            // Pass to DroneFlight object and its Id to View for use
            ViewBag.droneFlight = droneFlight;
            ViewBag.DroneFlightId = id;
            if (droneFlight == null)
            {
                ViewBag.ErrorMessage = "Drone Flight could not be found.";
                return View("~/Views/ErrorPage/Error.cshtml");
                //return HttpNotFound();
            }
            return View(qualityReport);
        }

        public ActionResult CTRLPoints(int? id)
        {
            if (id == null)
            {
                ViewBag.ErrorMessage = "Please specify a Drone Flight in your URL.";
                return View("~/Views/ErrorPage/Error.cshtml");
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // Get the appropriate drone flight per id
            DroneFlight droneFlight = db.DroneFlights.Find(id);
            // Pass to DroneFlight object and its Id to View for use
            ViewBag.droneFlight = droneFlight;
            ViewBag.DroneFlightId = id;
            if (droneFlight == null)
            {
                ViewBag.ErrorMessage = "Drone Flight could not be found.";
                return View("~/Views/ErrorPage/Error.cshtml");
                //return HttpNotFound();
            }
            return View(droneFlight.CTRLPoints.ToList());
        }

        public ActionResult GCPPoints(int? id)
        {
            if (id == null)
            {
                ViewBag.ErrorMessage = "Please specify a Drone Flight in your URL.";
                return View("~/Views/ErrorPage/Error.cshtml");
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // Get the appropriate drone flight per id
            DroneFlight droneFlight = db.DroneFlights.Find(id);
            // Pass to DroneFlight object and its Id to View for use
            ViewBag.droneFlight = droneFlight;
            ViewBag.DroneFlightId = id;
            if (droneFlight == null)
            {
                ViewBag.ErrorMessage = "Drone Flight could not be found.";
                return View("~/Views/ErrorPage/Error.cshtml");
                //return HttpNotFound();
            }
            return View(droneFlight.GroundControlPoints.ToList());
        }

        public ActionResult TFW(int? id)
        {
            if (id == null)
            {
                ViewBag.ErrorMessage = "Please specify a Drone Flight in your URL.";
                return View("~/Views/ErrorPage/Error.cshtml");
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // Get the appropriate drone flight per id
            DroneFlight droneFlight = db.DroneFlights.Find(id);
            // Pass to DroneFlight object and its Id to View for use
            ViewBag.droneFlight = droneFlight;
            ViewBag.DroneFlightId = id;
            if (droneFlight == null)
            {
                ViewBag.ErrorMessage = "Drone Flight could not be found.";
                return View("~/Views/ErrorPage/Error.cshtml");
                //return HttpNotFound();
            }
            return View(droneFlight.TFW);
        }

        public ActionResult Map(int? id)
        {
            if (id == null)
            {
                ViewBag.ErrorMessage = "Please specify a Drone Flight in your URL.";
                return View("~/Views/ErrorPage/Error.cshtml");
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // Get the appropriate drone flight per id
            DroneFlight droneFlight = db.DroneFlights.Find(id);
            // Pass to DroneFlight object and its Id to View for use
            ViewBag.droneFlight = droneFlight;
            ViewBag.DroneFlightId = id;
            if (droneFlight == null)
            {
                ViewBag.ErrorMessage = "Drone Flight could not be found.";
                return View("~/Views/ErrorPage/Error.cshtml");
                //return HttpNotFound();
            }
            return View(droneFlight);
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
