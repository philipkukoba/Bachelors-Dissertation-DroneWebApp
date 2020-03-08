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
            var droneFlights = db.DroneFlights.Include(d => d.Drone).Include(d => d.Pilot);
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
        public ActionResult Create()
        {
            ViewBag.DroneId = new SelectList(db.Drones, "DroneId", "DroneName");
            ViewBag.PilotId = new SelectList(db.Pilots, "PilotId", "PilotName");
            return View();
        }

        // POST: DroneFlights/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FlightId, DroneId, PilotId, Location, Date, hasTFW, hasGCPs, hasCTRLs, hasDepInfo, hasDestInfo, hasQR, hasXYZ, hasDroneLog")] DroneFlight droneFlight)
        {
            if (ModelState.IsValid)
            {
                db.DroneFlights.Add(droneFlight);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
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
            ViewBag.DroneId = new SelectList(db.Drones, "DroneId", "DroneName", droneFlight.DroneId);
            ViewBag.PilotId = new SelectList(db.Pilots, "PilotId", "PilotName", droneFlight.PilotId);
            return View(droneFlight);
        }

        // POST: DroneFlights/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FlightId, DroneId, PilotId, Location, Date, hasTFW, hasGCPs, hasCTRLs, hasDepInfo, hasDestInfo, hasQR, hasXYZ, hasDroneLog")] DroneFlight droneFlight)
        {
            if (ModelState.IsValid)
            {
                db.Entry(droneFlight).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DroneId = new SelectList(db.Drones, "DroneId", "DroneName", droneFlight.DroneId);
            ViewBag.PilotId = new SelectList(db.Pilots, "PilotId", "PilotName", droneFlight.PilotId);
            return View(droneFlight);
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
            catch(Exception ex)
            {
                ViewBag.ErrorDroneFlightDelete = "Cannot delete this Drone Flight.";
                return View(droneFlight);
            }
            
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
