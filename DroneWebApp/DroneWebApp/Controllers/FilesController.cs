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
        static List<string> fileNames;
        static List<bool> parseResults;
        static string currentFileName;
        static bool currentParseResult;
        static int totalFilesToParse = 0;
        static int filesLeft = 0;

        // Constructor
        public FilesController(DbContext db)
        {
            Db = (DroneDBEntities)db;
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
            return View();
        }

        [HttpPost]
        public ActionResult Index(int? id, List<HttpPostedFileBase> files)
        {
            // How many files to be read?
            totalFilesToParse = files.Count;
            filesLeft = files.Count;
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

            System.Diagnostics.Debug.WriteLine("filestoBeRead: " + totalFilesToParse );
            // Lists to be returned to the frontend
            fileNames = new List<string>();
            parseResults = new List<bool>();

            foreach (HttpPostedFileBase file in files)
            {
                currentFileName = "";
                // Verify that the user selected a file
                var path = "";
                if (file != null && file.ContentLength > 0)
                {
                    // extract only the filename
                    currentFileName = Path.GetFileName(file.FileName);
                    // add file name to the list of files
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
                        parseResults.Add(currentParseResult);
                    }
                    else
                    {
                        currentParseResult = creator.GetParser(fileExtension, path, (int)id);
                        parseResults.Add(currentParseResult);
                    }
                }

                filesLeft--;
            }
            return View();
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
            
            IFactory factory = null;

            if (extension.Equals("csv"))
            {
                factory = new FactoryCSV();
                if (type.Equals("drone"))
                {
                    factory.CreateDroneLog((int)id, Db, HttpContext);
                }
                else if (type.Equals("pilot"))
                {
                    factory.CreatePilotLog((int)id, Db, HttpContext);
                }
            }
            else if (extension.Equals("pdf"))
            {
                factory = new FactoryPDF();
                if (type.Equals("drone"))
                {
                    factory.CreateDroneLog((int)id, Db, HttpContext);
                }
                else if (type.Equals("pilot"))
                {
                    factory.CreatePilotLog((int)id, Db, HttpContext);
                }
            }

            return View();
        }

        [HttpGet]
        public int GetTotalFiles()
        {
            System.Diagnostics.Debug.WriteLine("filestoBeRead in GetTotalFiles: " + totalFilesToParse);
            return totalFilesToParse;
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
            //data projection
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
        public HttpResponseMessage GetResultsAndFileNames()
        {
            //data projection
            var result = (new
            {
                
            });

            //config to set to json 
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(result));
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return response;
        }
    }
}