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
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DroneWebApp.Controllers
{
    public class PilotsController : Controller
    {
        private DroneDBEntities db;
        private ApplicationDbContext applicationDb = new ApplicationDbContext();

        public PilotsController(DbContext db)
        {
            this.db = (DroneDBEntities) db;
        }

        // GET: Pilots
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(applicationDb));
                var s = UserManager.GetRoles(user.GetUserId());
                if (s[0].ToString() == "Admin" || s[0].ToString() == "User")
                {
                    return View("Index", db.Pilots.ToList());
                }
            }
            ViewBag.ErrorMessage = "Please make sure you are logged in";
            return View("~/Views/ErrorPage/Error.cshtml");
        }

        // GET: Pilots/Details/5
        public ActionResult Details(int? id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(applicationDb));
                var s = UserManager.GetRoles(user.GetUserId());
                if (s[0].ToString() == "Admin" || s[0].ToString() == "User")
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
            }
            ViewBag.ErrorMessage = "Please make sure you are logged in";
            return View("~/Views/ErrorPage/Error.cshtml");
        }

        // GET: Pilots/Create
        public ActionResult Create()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(applicationDb));
                var s = UserManager.GetRoles(user.GetUserId());
                if (s[0].ToString() == "Admin")
                {
                    return View("Create");
                }
            }
            ViewBag.ErrorMessage = "Please make sure you are logged in as administrator";
            return View("~/Views/ErrorPage/Error.cshtml");
        }

        // POST: Pilots/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PilotId,PilotName,Street,ZIP,City,Country,Phone,LicenseNo,Email,EmergencyPhone")] Pilot pilot)
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(applicationDb));
                var s = UserManager.GetRoles(user.GetUserId());
                if (s[0].ToString() == "Admin")
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
            }
            ViewBag.ErrorMessage = "Please make sure you are logged in as administrator";
            return View("~/Views/ErrorPage/Error.cshtml");
        }

        // GET: Pilots/Edit/5
        public ActionResult Edit(int? id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(applicationDb));
                var s = UserManager.GetRoles(user.GetUserId());
                if (s[0].ToString() == "Admin")
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
            }
            ViewBag.ErrorMessage = "Please make sure you are logged in as administrator";
            return View("~/Views/ErrorPage/Error.cshtml");
        }

        // POST: Pilots/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PilotId,PilotName,Street,ZIP,City,Country,Phone,LicenseNo,Email,EmergencyPhone")] Pilot pilot)
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(applicationDb));
                var s = UserManager.GetRoles(user.GetUserId());
                if (s[0].ToString() == "Admin")
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
            }
            ViewBag.ErrorMessage = "Please make sure you are logged in as administrator";
            return View("~/Views/ErrorPage/Error.cshtml");
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
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(applicationDb));
                var s = UserManager.GetRoles(user.GetUserId());
                if (s[0].ToString() == "Admin")
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
            }
            ViewBag.ErrorMessage = "Please make sure you are logged in as administrator";
            return View("~/Views/ErrorPage/Error.cshtml");
        }

        // POST: Pilots/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(applicationDb));
                var s = UserManager.GetRoles(user.GetUserId());
                if (s[0].ToString() == "Admin")
                {
                    Pilot pilot = db.Pilots.Find(id);
                    ViewBag.NumberOfFlights = pilot.DroneFlights.Count;
                    try
                    {
                        db.Pilots.Remove(pilot);
                        db.SaveChanges();
                    }
                    catch (Exception)
                    {
                        ViewBag.ErrorPilotDelete = "Cannot delete this Pilot. " + pilot.PilotName + " is assigned to one or more Flights.";
                        return View(pilot);
                    }
                    return RedirectToAction("Index");
                }
            }
            ViewBag.ErrorMessage = "Please make sure you are logged in as administrator";
            return View("~/Views/ErrorPage/Error.cshtml");
        }

        // GET: Pilots/DroneFlights/5
        public ActionResult DroneFlights(int? id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(applicationDb));
                var s = UserManager.GetRoles(user.GetUserId());
                if (s[0].ToString() == "Admin" || s[0].ToString() == "User")
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
            }
            ViewBag.ErrorMessage = "Please make sure you are logged in";
            return View("~/Views/ErrorPage/Error.cshtml");
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
