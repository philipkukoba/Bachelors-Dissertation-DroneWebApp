using DroneWebApp.Models;
using DroneWebApp.Models.Helper;
using DroneWebApp.Models.SimpleFactoryPattern;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace DroneWebApp.Controllers
{
    // WIP
    public class FilesController : Controller
    {
        public DroneDBEntities Db { get; set; }
        private Creator creator;
        private readonly List<string> validExtensions = new List<string>(){ ".pdf", ".dat", ".txt", ".csv", ".xyz", ".tfw"};
    
        // Constructor
        public FilesController(DbContext db)
        {
            this.Db = (DroneDBEntities)db;
            creator = new Creator(Db);
            ViewBag.Status = true;
            ViewBag.showInitialMessage = true;
        }

        [HttpGet]
        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                ViewBag.ErrorMessage = "Please specify a Drone Flight in your URL.";
                return View("~/Views/ErrorPage/Error.cshtml");
            }
            DroneFlight droneFlight = Db.DroneFlights.Find((int) id);
            ViewBag.FlightId = (int) id;
            ViewBag.Location = droneFlight.Location;
            ViewBag.Date = droneFlight.Date.ToString("dd/MM/yyyy");
            return View();
        }

        [HttpGet]
        public double getUploadStatus() {
            System.Diagnostics.Debug.WriteLine(Helper.progress + "\n");
            return Helper.progress; 
        }


        //Single File Upload
        [HttpPost]
        public ActionResult Index(int? id, HttpPostedFileBase files)
        {
            // Check if an id was submitted & whether a drone flight with this id exists
            DroneFlight droneFlight = Db.DroneFlights.Find(id);
            if (id == null)
            {
                ViewBag.ErrorMessage = "Please specify a Drone Flight in your URL.";
                return View("~/Views/ErrorPage/Error.cshtml");
            }
            else if (droneFlight == null)
            {
                ViewBag.ErrorMessage = "This Drone Flight does not exist.";
                return View("~/Views/ErrorPage/Error.cshtml");
            }

            // Verify that the user selected a file
            var path = "";
            if (files != null && files.ContentLength > 0)
            {
                // extract only the filename
                var fileName = Path.GetFileName(files.FileName);
                // store the file inside ~/App_Data/uploads folder
                path = Path.Combine(Server.MapPath("~/files"), fileName);
                files.SaveAs(path);              
            }

            string file_name = files.FileName;
            string fileExtension = file_name.Substring(file_name.Length - 4);

            // Verify that the user's file is an appropriate filetype
            if (!validExtensions.Contains(fileExtension))
            {
                ViewBag.ErrorMessage = "This is not a valid filetype. Please choose an appropriate filetype.";
            }
            else
            {
                // Parsing
                ViewBag.Status = creator.GetParser(fileExtension, path, (int)id);

                if (file_name.Contains("FLY")) // DAT-bestanden zijn voorlopig csv en moeten dus juist afgehandeld worden
                {
                    ViewBag.Status = creator.GetParser(".dat", path, (int)id);
                }
                ViewBag.FileName = file_name;
            }
            ViewBag.showInitialMessage = false;
            System.Diagnostics.Debug.WriteLine("*****************************");
            return View();
        }
    }
}