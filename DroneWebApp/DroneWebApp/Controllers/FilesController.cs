using DroneWebApp.Models;
using DroneWebApp.Models.DataExport;
using DroneWebApp.Models.Helper;
using DroneWebApp.Models.SimpleFactoryPattern;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Mvc;


namespace DroneWebApp.Controllers
{
    public class FilesController : Controller
    {
        public DroneDBEntities Db { get; set; }
        private Creator creator;
        private readonly List<string> validExtensions = new List<string>(){ ".pdf", ".dat", ".txt", ".csv", ".xyz", ".tfw", ".jpg"};
        // Parsing variables
        private static List<string> fileNames; // list that keeps track of the files' names
        private static List<bool> parseResults; // list that keeps track of the success or failure of those files
        private static Dictionary<string, bool> results;
        private static string currentFileName; // the current file that is being processed
        private static bool currentParseResult; // the current parse result
        private static int totalFilesToParse = 0; // the total amount of files that must be parsed
        private static int filesLeft = 0; // the amount of files that still have to be parsed

        // Constructor
        public FilesController(DbContext db)
        {
            Db = (DroneDBEntities)db;
            creator = new Creator(Db);
            System.Diagnostics.Debug.WriteLine("FilesController constructor");
        }

        [HttpGet]
        public ActionResult Index(int? id)
        {
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
            ViewBag.FlightId = (int) id;
            ViewBag.Location = droneFlight.Location;
            string date = "NA";
            if (droneFlight.Date != null)
            {
                date = ((DateTime)droneFlight.Date).ToString("dd/MM/yyyy, HH:mm:ss");
            }
            ViewBag.Date = date;
            return View("Index");
        }

        [HttpPost]
        public ActionResult Index(int? id, List<HttpPostedFileBase> files)
        {
            // How many files must be parsed?
            totalFilesToParse = files.Count;
            filesLeft = totalFilesToParse;
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

            System.Diagnostics.Debug.WriteLine("Total Files to Parse: " + totalFilesToParse );

            // Lists to be returned to the front-end
            //fileNames = new List<string>();
            //parseResults = new List<bool>();
            results = new Dictionary<string, bool>();

            foreach (HttpPostedFileBase file in files)
            {
                // Verify that the file provided exists
                if (file != null)
                {
                    currentFileName = "";
                    // Verify that the user selected a file
                    var path = "";
                    if (file != null && file.ContentLength > 0)
                    {
                        // extract only the filename
                        currentFileName = Path.GetFileName(file.FileName); // set the current file name
                                                                           // add this file name to the list of files
                        fileNames.Add(currentFileName);
                        
                        // store the file inside ~/files/ folder
                        path = Path.Combine(Server.MapPath("~/files"), currentFileName);
                        file.SaveAs(path);
                    }

                    string fileExtension = currentFileName.Substring(currentFileName.Length - 4);
                    // Verify that the user's file is an appropriate filetype
                    if (!validExtensions.Contains(fileExtension.ToLower())) //set lowercase
                    {
                        ViewBag.ErrorMessage = "This is not a valid filetype. Please choose an appropriate filetype.";
                        return View("~/Views/ErrorPage/Error.cshtml");
                    }
                    else
                    {
                        // Parsing
                        if (currentFileName.Contains("FLY")) // DAT-bestanden zijn voorlopig csv en moeten dus juist afgehandeld worden
                        {
                            currentParseResult = creator.GetParser(".dat", path, (int)id);
                            //parseResults.Add(currentParseResult);
                            results.Add(currentFileName, currentParseResult);
                        }
                        else
                        {
                            currentParseResult = creator.GetParser(fileExtension, path, (int)id);
                            //parseResults.Add(currentParseResult);
                            results.Add(currentFileName, currentParseResult);
                        }
                    }
                }
                filesLeft--;
            }
            return View("Index");
        }

        public ActionResult Export(int? id, string extension, string type)
        {
            if (id == null)
            {
                ViewBag.ErrorMessage = "Please specify an id in your URL.";
                return View("~/Views/ErrorPage/Error.cshtml");
            }
            if (extension == null || type == null)
            {
                ViewBag.ErrorMessage = "Please specify an extension and/or type in your URL.";
                return View("~/Views/ErrorPage/Error.cshtml");
            }

            IExport export = null;

            if (extension.Equals("csv"))
            {
                export = new ExportCSV();
                if (type.Equals("drone"))
                {
                    export.CreateDroneLog((int)id, Db, HttpContext);
                }
                else if (type.Equals("pilot"))
                {
                    export.CreatePilotLog((int)id, Db, HttpContext);
                }
            }
            else if (extension.Equals("pdf"))
            {
                export = new ExportPDF();
                if (type.Equals("drone"))
                {
                    export.CreateDroneLog((int)id, Db, HttpContext);
                }
                else if (type.Equals("pilot"))
                {
                    export.CreatePilotLog((int)id, Db, HttpContext);
                }
            }

            return View("Export");
        }

        [HttpGet]
        public ActionResult GetStatus()
        {
            int parseResultToInt;
            if (currentParseResult)
            {
                parseResultToInt = 1;
            }
            else
            {
                parseResultToInt = 0;
            }

            var result = (new
            {
                currProgress = Helper.progress,
                currParseResult = parseResultToInt,
                currFileName = currentFileName,
                currFilesLeft = filesLeft,
            });

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // gets the list of parsing results and list of file names
        // todo: return a list of the files that failed + their name
        [HttpGet]
        public ActionResult GetResultsAndFileNames()
        {
            List<string> failed = new List<string>();
            foreach (KeyValuePair<string, bool> entry in results)
            {
                if(entry.Value == false)
                {
                    failed.Add(entry.Key);
                }
            }

            var result = (new
            {
                failedFiles = failed,
            });

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
