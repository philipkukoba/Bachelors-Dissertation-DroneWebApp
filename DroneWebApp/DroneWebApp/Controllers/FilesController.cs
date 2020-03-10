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
        public ActionResult Index()
        {
            return View();
        }

        //Single File Upload
        [HttpPost]
        public ActionResult Index(HttpPostedFileBase files)
        {            
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

            DbContext dbx = new DroneDBEntities();
            Creator c = new Creator(dbx);

            c.GetParser(fileExtension, path, 1);
            //c.GetParser(".dat", path, 1); //dat testing 

            System.Diagnostics.Debug.WriteLine("net voor de return");
            return View(); //gwn op zelfde pagina blijven
        }
    }
}