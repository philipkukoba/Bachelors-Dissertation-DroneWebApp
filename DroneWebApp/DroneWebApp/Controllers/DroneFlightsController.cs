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
            return View("Index", droneFlights.ToList());
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
            return View("Details", droneFlight);
        }

        // GET: DroneFlights/Create
        public ActionResult Create(int? pilotId, int? projectId, int? droneId)
        {
            if(pilotId != null)
            {
                ViewBag.PilotId = new SelectList(db.Pilots, "PilotId", "PilotName", pilotId);
            }
            else
            {
                ViewBag.PilotId = new SelectList(db.Pilots, "PilotId", "PilotName");
            }
            if (droneId != null)
            {
                ViewBag.DroneId = new SelectList(db.Drones, "DroneId", "DroneName", droneId);
            }
            else
            {
                ViewBag.DroneId = new SelectList(db.Drones, "DroneId", "DroneName");
            }
            if (projectId != null)
            {
                ViewBag.ProjectId = new SelectList(db.Projects, "ProjectId", "ProjectCode", projectId);
            }
            else
            {
                ViewBag.ProjectId = new SelectList(db.Projects, "ProjectId", "ProjectCode");
            }
            ViewBag.DroneId = new SelectList(db.Drones, "DroneId", "DroneName");
            return View("Create");
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
                if (string.IsNullOrWhiteSpace(droneFlight.Location))
                {
                    droneFlight.Location = "TBD"; // TBD = to be determined; indicates no location was set during creation of flight
                }
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
            return View("Edit", droneFlight);
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
            return View("Delete", droneFlight);
        }

        // POST: DroneFlights/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            DroneFlight droneFlight = db.DroneFlights.Find(id);

            // Calculate the flight time of this drone flight
            TimeSpan totalTime = new TimeSpan(0, 0, 0, 0);
            if(droneFlight.hasDepInfo && droneFlight.hasDestInfo)
            {
                totalTime = totalTime.Add(((TimeSpan)droneFlight.DestinationInfo.UTCTime).Subtract((TimeSpan)droneFlight.DepartureInfo.UTCTime));
                // Update the threshold time for the drone that was assigned to this flight to account for this deletion
                droneFlight.Drone.nextTimeCheck = droneFlight.Drone.nextTimeCheck - (long)totalTime.TotalSeconds;
                droneFlight.Drone.needsCheckUp = false; // Reset to false; Helper.UpdateTotalDroneFlightTime will re-evaluate whether or not this needs to stay false
            }
            // Remove this drone flight
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
            return View("QualityReport", qualityReport);
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
            return View("CTRLPoints", droneFlight.CTRLPoints.ToList());
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
            return View("GCPPoints", droneFlight.GroundControlPoints.ToList());
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
            return View("TFW", droneFlight.TFW);
        }

        public ActionResult RawImages(int? id)
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
            return View("RawImages", droneFlight.RawImages);
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
