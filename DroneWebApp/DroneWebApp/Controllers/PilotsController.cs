using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DroneWebApp.Models;

namespace DroneWebApp.Controllers
{
    public class PilotsController : Controller
    {
        private DroneDBEntities db;

        public PilotsController(DbContext db)
        {
            this.db = (DroneDBEntities) db;
        }

        // GET: Pilots
        public ActionResult Index()
        {
            return View("Index", db.Pilots.ToList());
        }

        // GET: Pilots/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                ViewBag.ErrorMessage = "Please specify a Pilot in your URL.";
                return View("~/Views/ErrorPage/Error.cshtml");
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pilot pilot = db.Pilots.Find(id);
            if (pilot == null)
            {
                ViewBag.ErrorMessage = "Pilot could not be found.";
                return View("~/Views/ErrorPage/Error.cshtml");
                //return HttpNotFound();
            }
            return View("Details", pilot);
        }

        // GET: Pilots/Create
        public ActionResult Create()
        {
            return View("Create");
        }

        // POST: Pilots/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PilotId,PilotName,Street,ZIP,City,Country,Phone,LicenseNo,Email,EmergencyPhone")] Pilot pilot)
        {
            if (ModelState.IsValid)
            {
                bool pilotAlreadyExists = db.Pilots.Any(x => x.PilotName == pilot.PilotName);
                if (pilotAlreadyExists)
                {
                    ViewBag.ErrorPilotCreate = "Pilot already exists. Please choose a different name.";
                    return View(pilot);
                }
                
                db.Pilots.Add(pilot);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(pilot);
        }

        // GET: Pilots/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                ViewBag.ErrorMessage = "Please specify a Pilot in your URL.";
                return View("~/Views/ErrorPage/Error.cshtml");
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pilot pilot = db.Pilots.Find(id);
            if (pilot == null)
            {
                ViewBag.ErrorMessage = "Pilot could not be found.";
                return View("~/Views/ErrorPage/Error.cshtml");
                //return HttpNotFound();
            }
            return View("Edit", pilot);
        }

        // POST: Pilots/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PilotId,PilotName,Street,ZIP,City,Country,Phone,LicenseNo,Email,EmergencyPhone")] Pilot pilot)
        {
            if (ModelState.IsValid)
            {
                Pilot p = db.Pilots.Find(pilot.PilotId);
                UpdatePilotFields(pilot, p);
                db.Entry(p).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(pilot);
        }

        // Update the fields of the DroneFlight that has been found by FlightId with the fields of the posted DroneFlight, a.k.a. the drone flight 
        // the user has submitted
        private static void UpdatePilotFields(Pilot postedPilot, Pilot p)
        {
            p.PilotName = postedPilot.PilotName;
            p.Street = postedPilot.Street;
            p.ZIP = postedPilot.ZIP;
            p.City = postedPilot.City;
            p.Country = postedPilot.Country;
            p.Phone = postedPilot.Phone;
            p.LicenseNo = postedPilot.LicenseNo;
            p.Email = postedPilot.Email;
            p.EmergencyPhone = postedPilot.EmergencyPhone;
        }

        // GET: Pilots/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                ViewBag.ErrorMessage = "Please specify a Pilot in your URL.";
                return View("~/Views/ErrorPage/Error.cshtml");
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pilot pilot = db.Pilots.Find(id);
            ViewBag.NumberOfFlights = pilot.DroneFlights.Count;
            if (pilot == null)
            {
                ViewBag.ErrorMessage = "Pilot could not be found.";
                return View("~/Views/ErrorPage/Error.cshtml");
                //return HttpNotFound();
            }
            return View("Delete", pilot);
        }

        // POST: Pilots/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            Pilot pilot = db.Pilots.Find(id);
            ViewBag.NumberOfFlights = pilot.DroneFlights.Count;
            try
            {
                db.Pilots.Remove(pilot);
                db.SaveChanges();
            }
            catch(Exception) {
                ViewBag.ErrorPilotDelete = "Cannot delete this Pilot. " + pilot.PilotName +" is assigned to one or more Flights.";
                return View(pilot);
            }
            return RedirectToAction("Index");
        }

        // GET: Pilots/DroneFlights/5
        public ActionResult DroneFlights(int? id)
        {
            if (id == null)
            {
                ViewBag.ErrorMessage = "Please specify a Pilot in your URL.";
                return View("~/Views/ErrorPage/Error.cshtml");
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pilot pilot = db.Pilots.Find(id);
            if (pilot == null)
            {
                ViewBag.ErrorMessage = "Pilot could not be found.";
                return View("~/Views/ErrorPage/Error.cshtml");
                //return HttpNotFound();
            }
            ViewBag.Pilot = pilot.PilotName;
            ViewBag.PilotId = id;
            return View("DroneFlights", pilot.DroneFlights.ToList());
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
