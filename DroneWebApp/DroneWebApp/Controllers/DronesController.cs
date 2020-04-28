using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DroneWebApp.Models;
using DroneWebApp.Models.Helper;

namespace DroneWebApp.Controllers
{
    public class DronesController : Controller
    {
        private DroneDBEntities db;
        private static TimeSpan thresholdTime = new TimeSpan(0, 5, 0); // The amount of time after which a drone has to be checked to verify it is safe to fly
                                                                       // ToDo: change 5 minutes into 50 hours and correctly do calculations above 24 hours (with days in TimeSpan perhaps)              
        public DronesController(DbContext db)
        {
            this.db = (DroneDBEntities) db;
        }

        // GET: Drones
        public ActionResult Index()
        {
            return View(db.Drones.ToList());
        }

        // GET: Drones/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                ViewBag.ErrorMessage = "Please specify a Drone in your URL.";
                return View("~/Views/ErrorPage/Error.cshtml");
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Drone drone = db.Drones.Find(id);
            if (drone == null)
            {
                ViewBag.ErrorMessage = "Pilot could not be found.";
                return View("~/Views/ErrorPage/Error.cshtml");
                //return HttpNotFound();
            }
            return View(drone);
        }

        // GET: Drones/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Drones/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DroneId,Registration,DroneType, DroneName")] Drone drone)
        {
            if (ModelState.IsValid)
            {
                if(drone.DroneName == null)
                {
                    drone.DroneName = drone.DroneType + ":" + drone.Registration;
                }
                drone.TotalFlightTime = new TimeSpan(0, 0, 0);
                drone.needsCheckUp = false;
                drone.nextTimeCheck = thresholdTime;

                db.Drones.Add(drone);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(drone);
        }

        // GET: Drones/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                ViewBag.ErrorMessage = "Please specify a Drone in your URL.";
                return View("~/Views/ErrorPage/Error.cshtml");
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Drone drone = db.Drones.Find(id);
            if (drone == null)
            {
                ViewBag.ErrorMessage = "Drone could not be found.";
                return View("~/Views/ErrorPage/Error.cshtml");
                //return HttpNotFound();
            }
            return View(drone);
        }

        // POST: Drones/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DroneId,Registration,DroneType,DroneName,needsCheckUp")] Drone drone)
        {
            if (ModelState.IsValid)
            {
                Drone dr = db.Drones.Find(drone.DroneId);
                UpdateDroneFields(drone, dr);
                db.Entry(dr).State = EntityState.Modified;
                db.SaveChanges();
                // Update the total time drones have flown in case the drone flight's drone has been changed by the user
                Helper.UpdateTotalDroneFlightTime(this.db);
                return RedirectToAction("Index");
            }
            return View(drone);
        }

        // Update the fields of the Drone that has been found by DroneId with the fields of the posted Drone, a.k.a. the drone 
        // the user has submitted
        private static void UpdateDroneFields(Drone postedDrone, Drone dr)
        {
            dr.Registration = postedDrone.Registration;
            dr.DroneType = postedDrone.DroneType;
            dr.DroneName = postedDrone.DroneName;
            if(postedDrone.needsCheckUp == false)
            {
                dr.needsCheckUp = postedDrone.needsCheckUp;
                dr.nextTimeCheck = ((TimeSpan)dr.TotalFlightTime).Add(thresholdTime);
            }
        }

        // GET: Drones/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                ViewBag.ErrorMessage = "Please specify a Drone in your URL.";
                return View("~/Views/ErrorPage/Error.cshtml");
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Drone drone = db.Drones.Find(id);
            
            if (drone == null)
            {
                ViewBag.ErrorMessage = "Drone could not be found.";
                return View("~/Views/ErrorPage/Error.cshtml");
                //return HttpNotFound();
            }
            // Count its total flights
            ViewBag.TotalFlights = drone.DroneFlights.Count;
            return View(drone);
        }

        // POST: Drones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            Drone drone = db.Drones.Find(id);
            try
            {
                db.Drones.Remove(drone);
                db.SaveChanges();
            }
            catch (Exception)
            {
                ViewBag.ErrorDroneDelete = "Cannot delete this Drone. " + drone.DroneName + " is assigned to one or more Flight.";
                return View(drone);
            }
            // Count its total flights
            ViewBag.TotalFlights = drone.DroneFlights.Count;
            return RedirectToAction("Index");
        }

        // GET: Drones/DroneFlights/5
        public ActionResult DroneFlights(int? id)
        {
            if (id == null)
            {
                ViewBag.ErrorMessage = "Please specify a Drone in your URL.";
                return View("~/Views/ErrorPage/Error.cshtml");
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Drone drone = db.Drones.Find(id);
            if (drone == null)
            {
                ViewBag.ErrorMessage = "Drone could not be found.";
                return View("~/Views/ErrorPage/Error.cshtml");
                //return HttpNotFound();
            }
            ViewBag.DroneName = drone.DroneName;
            ViewBag.DroneId = id;
            return View(drone.DroneFlights.ToList());
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
