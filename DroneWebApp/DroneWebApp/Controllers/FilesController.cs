using DroneWebApp.Models;
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
        private Creator creator;
        private List<string> validExtensions = new List<string>(){ ".pdf", ".dat", ".txt", ".csv", ".xyz", ".tfw"};

        public FilesController(DbContext db)
        {
            this.Db = (DroneDBEntities)db;
            creator = new Creator(Db);
        }
        public DroneDBEntities Db { get; set; }

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

        //Single File Upload
        [HttpPost]
        public ActionResult Index(int? id, HttpPostedFileBase files)
        {
            if (id == null)
            {
                ViewBag.ErrorMessage = "Please specify a Drone Flight in your URL.";
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

            string filename = files.FileName;
            string fileExtension = filename.Substring(filename.Length - 4);

            if (!validExtensions.Contains(fileExtension))
            {
                ViewBag.ErrorMessage = "This is not a valid filetype. Please choose an appropriate filetype.";
            }

            creator.GetParser(fileExtension, path, (int)id);

            if (filename.Contains("FLY")) // DAT-bestanden zijn voorlopig csv en moeten dus juist afgehandeld worden
            {
                creator.GetParser(".dat", path, (int)id);
            }
            ViewBag.Success = "File has been successfully added to your Flight.";
            return View();
        }
    }
}