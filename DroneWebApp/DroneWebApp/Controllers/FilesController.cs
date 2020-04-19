﻿using DroneWebApp.Models;
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
        static string fileName;
        static bool parseResult = false;

        // Constructor
        public FilesController(DbContext db)
        {
            this.Db = (DroneDBEntities)db;
            creator = new Creator(Db);
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
            //System.Diagnostics.Debug.WriteLine("Index regular called");
            return View();
        }

        //Single File Upload
        [HttpPost]
        public ActionResult Index(int? id, HttpPostedFileBase files)
        {
            // Check if an id was submitted & whether a drone flight with this id exists            // ********************************
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
                fileName = Path.GetFileName(files.FileName);
                System.Diagnostics.Debug.WriteLine("****** File name: " + fileName);
                // store the file inside ~/App_Data/uploads folder
                path = Path.Combine(Server.MapPath("~/files"), fileName);
                files.SaveAs(path);              
            }

            //fileName = files.FileName;
            string fileExtension = fileName.Substring(fileName.Length - 4);
            // Verify that the user's file is an appropriate filetype                           // *****************************
            if (!validExtensions.Contains(fileExtension))
            {
                ViewBag.ErrorMessage = "This is not a valid filetype. Please choose an appropriate filetype.";
                return View("~/Views/ErrorPage/Error.cshtml");
            }
            else
            {
                // Parsing
                if (fileName.Contains("FLY")) // DAT-bestanden zijn voorlopig csv en moeten dus juist afgehandeld worden
                {
                    parseResult = creator.GetParser(".dat", path, (int)id);
                }
                else
                {
                    parseResult = creator.GetParser(fileExtension, path, (int)id);
                }
            }
            return View();
        }

        // gets the progress value of the file parsing
        [HttpGet]
        public double GetProgressStatus()
        {
            return Helper.progress;
        }

        // gets the result value of the parsing
        // returns true if a file was successfully read; 
        // returns false if a file was not read because it already existed
        [HttpGet]
        public int GetParseResult()
        {
            if(parseResult)
            {
                return 1;
            }
            return 0;
        }

        [HttpGet]
        public string GetFileName()
        {
            return fileName;
        }
    }
}