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
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DroneWebApp.Controllers
{
    public class DronesController : Controller
    {
        private DroneDBEntities db;
        private ApplicationDbContext applicationDb = new ApplicationDbContext();
        // thresholdTime is the amount of time after which a drone has to be checked to verify it is safe to fly, from its last check-up (or from 0 flight time)   
        private static TimeSpan thresholdTime = new TimeSpan(2, 2, 0, 0); // 2*24h + 2h = 50h

        public DronesController(DbContext db)
        {
            this.db = (DroneDBEntities) db;
        }

        // GET: Drones
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(applicationDb));
                var s = UserManager.GetRoles(user.GetUserId());
                if (s[0].ToString() == "Admin" || s[0].ToString() == "User")
                {
                    return View("Index", db.Drones.ToList());
                }
            }
            ViewBag.ErrorMessage = "Please make sure you are logged in";
            return View("~/Views/ErrorPage/Error.cshtml");
        }

        // GET: Drones/Details/5
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
                    return View("Details", drone);
                }
            }
            ViewBag.ErrorMessage = "Please make sure you are logged in";
            return View("~/Views/ErrorPage/Error.cshtml");
        }

        // GET: Drones/Create
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

        // POST: Drones/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DroneId,Registration,DroneType, DroneName, Notes")] Drone drone)
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
                        if (drone.DroneName == null)
                        {
                            drone.DroneName = drone.DroneType + ":" + drone.Registration;
                        }
                        drone.TotalFlightTime = 0; // no time flown yet
                        drone.needsCheckUp = false; // does not yet need a check-up

                        drone.nextTimeCheck = (long)thresholdTime.TotalSeconds; // Save the next time check as total amount of ticks (bigint in database, long in application).
                                                                                // This is because SQL Server cannot store times > 24 hours; solution: store time as seconds
                        db.Drones.Add(drone);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }

                    return View(drone);
                }
            }
            ViewBag.ErrorMessage = "Please make sure you are logged in as administrator";
            return View("~/Views/ErrorPage/Error.cshtml");
        }

        // GET: Drones/Edit/5
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
                    return View("Edit", drone);
                }
            }
            ViewBag.ErrorMessage = "Please make sure you are logged in as administrator";
            return View("~/Views/ErrorPage/Error.cshtml");
        }

        // POST: Drones/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DroneId,Registration,DroneType,DroneName,needsCheckUp, Notes")] Drone drone)
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
            }
            ViewBag.ErrorMessage = "Please make sure you are logged in as administrator";
            return View("~/Views/ErrorPage/Error.cshtml");
        }

        // Update the fields of the Drone that has been found by DroneId with the fields of the posted Drone, a.k.a. the drone 
        // the user has submitted
        private static void UpdateDroneFields(Drone postedDrone, Drone dr)
        {
            dr.Registration = postedDrone.Registration;
            dr.DroneType = postedDrone.DroneType;
            dr.DroneName = postedDrone.DroneName;
            if(string.IsNullOrWhiteSpace(postedDrone.DroneName))
            {
                dr.DroneName = postedDrone.DroneType + ":" + postedDrone.Registration;
            }
            else
            {
                dr.DroneName = postedDrone.DroneName;
            }
            if(postedDrone.needsCheckUp == true && dr.needsCheckUp == false) // This condition checks whether the user manually indicated that the drone NEEDS a check-up
            {
                dr.needsCheckUp = postedDrone.needsCheckUp; // drone needs a check-up
            }
            if(postedDrone.needsCheckUp == false) // This condition is true if the user has ticked the 'drone has been checked' box in the edit of a drone, indicating the drone was checked
            {
                dr.needsCheckUp = postedDrone.needsCheckUp;
                dr.nextTimeCheck = (long)dr.TotalFlightTime + (long)thresholdTime.TotalSeconds; // calculate the new next time check
            }
            dr.Notes = postedDrone.Notes;
        }

        // GET: Drones/Delete/5
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
                    return View("Delete", drone);
                }
            }
            ViewBag.ErrorMessage = "Please make sure you are logged in as administrator";
            return View("~/Views/ErrorPage/Error.cshtml");
        }

        // POST: Drones/Delete/5
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
            }
            ViewBag.ErrorMessage = "Please make sure you are logged in as administrator";
            return View("~/Views/ErrorPage/Error.cshtml");
        }

        // GET: Drones/DroneFlights/5
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
                    return View("DroneFlights", drone.DroneFlights.ToList());
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
