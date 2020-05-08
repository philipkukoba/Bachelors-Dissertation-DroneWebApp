using DroneWebApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DroneWebApp.Controllers
{
    public class MapController : Controller
    {
        private DroneDBEntities db;

        //Constructor
        public MapController(DbContext db)
        {
            this.db = (DroneDBEntities)db;
        }

        //public ActionResult ViewMap()
        //{
        //    return View(db.DroneFlights.ToList());
        //}

        public ActionResult ViewMap(int? id)
        {
            System.Diagnostics.Debug.WriteLine(id);
            if (id == null)
            {
                //ViewBag.ErrorMessage = "Please specify a Drone Flight in your URL.";
                //return View("~/Views/ErrorPage/Error.cshtml");
                ////return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                return View("ViewMap");
            }
            //else if (id == 0)
            //{
                
            //}
            else
            {
                DroneFlight droneFlight = db.DroneFlights.Find(id);
                if (droneFlight == null)
                {
                    ViewBag.ErrorMessage = "Drone Flight could not be found.";
                    return View("~/Views/ErrorPage/Error.cshtml");
                    //return HttpNotFound();
                }
                ViewBag.id = id;
            }
            return View("ViewMap"); //TODO: waarom droneflights meegeven? 
        }


        //public ActionResult ViewMapByID()
        //{
        //    return View();
        //}
    }
}