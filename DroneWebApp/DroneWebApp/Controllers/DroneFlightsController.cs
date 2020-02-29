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
        private DroneDBEntities db = new DroneDBEntities();

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
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DroneFlight droneFlight = db.DroneFlights.Find(id);
            if (droneFlight == null)
            {
                return HttpNotFound();
            }
            return View(droneFlight);
        }

        // GET: DroneFlights/Create
        public ActionResult Create()
        {
            ViewBag.DroneId = new SelectList(db.Drones, "DroneId", "DroneName");
            ViewBag.PilotName = new SelectList(db.Pilots, "PilotName", "PilotName");
            return View();
        }

        // POST: DroneFlights/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FlightId,DroneId,Location,Date,PilotName, hasTFW, hasGCP, hasDepInfo, hasDestInfo, hasQR, hasXYZ")] DroneFlight droneFlight)
        {
            if (ModelState.IsValid)
            {
                db.DroneFlights.Add(droneFlight);
                db.SaveChanges(); // TODO: check for dup keys
                return RedirectToAction("Index");
            }

            ViewBag.DroneId = new SelectList(db.Drones, "DroneId", "DroneName", droneFlight.DroneId);
            ViewBag.PilotName = new SelectList(db.Pilots, "PilotName", "PilotName", droneFlight.PilotName);
            return View(droneFlight);
        }

        // GET: DroneFlights/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DroneFlight droneFlight = db.DroneFlights.Find(id);
            if (droneFlight == null)
            {
                return HttpNotFound();
            }
            ViewBag.DroneId = new SelectList(db.Drones, "DroneId", "Registration", droneFlight.DroneId);
            ViewBag.PilotName = new SelectList(db.Pilots, "PilotName", "PilotName", droneFlight.PilotName);
            return View(droneFlight);
        }

        // POST: DroneFlights/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FlightId,DroneId,Location,Date,PilotName, hasTFW, hasGCP, hasDepInfo, hasDestInfo, hasQR, hasXYZ")] DroneFlight droneFlight)
        {
            if (ModelState.IsValid)
            {
                db.Entry(droneFlight).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DroneId = new SelectList(db.Drones, "DroneId", "Registration", droneFlight.DroneId);
            ViewBag.PilotName = new SelectList(db.Pilots, "PilotName", "Street", droneFlight.PilotName);
            return View(droneFlight);
        }

        // GET: DroneFlights/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DroneFlight droneFlight = db.DroneFlights.Find(id);
            if (droneFlight == null)
            {
                return HttpNotFound();
            }
            return View(droneFlight);
        }

        // POST: DroneFlights/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            DroneFlight droneFlight = db.DroneFlights.Find(id);
            db.DroneFlights.Remove(droneFlight);
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
