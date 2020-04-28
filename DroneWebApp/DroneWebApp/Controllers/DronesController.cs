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
                drone.needsCheckUp = false;

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
        public ActionResult Edit([Bind(Include = "DroneId,Registration,DroneType,DroneName,TotalFlightTime,needsCheckUp,hadCheckUp")] Drone drone)
        {
            if (ModelState.IsValid)
            {
                Drone dr = db.Drones.Find(drone.DroneId);
                UpdateDroneFields(drone, dr);
                db.Entry(dr).State = EntityState.Modified;
                // Update the total time drones have flown in case the drone flight's drone has been changed by the user
                Helper.UpdateTotalDroneFlightTime(this.db);
                db.SaveChanges();
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
            dr.TotalFlightTime = postedDrone.TotalFlightTime;
            dr.needsCheckUp = postedDrone.needsCheckUp;
            dr.hadCheckUp = postedDrone.hadCheckUp;
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
