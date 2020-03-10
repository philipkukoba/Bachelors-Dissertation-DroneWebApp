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
        public FilesController(DbContext db)
        {
            this.Db = (DroneDBEntities)db;
            creator = new Creator(Db);
        }
        public DroneDBEntities Db { get; set; }

        [HttpGet]
        public ActionResult Index()
        {
            System.Diagnostics.Debug.WriteLine("entered page through index()");
            return View();
        }

        //Single File Upload
        [HttpPost]
        public ActionResult Index(HttpPostedFileBase files)
        {
            System.Diagnostics.Debug.WriteLine("entered page through index(http.. files)");
            
            // Verify that the user selected a file
            var path = "";
            if (files != null && files.ContentLength > 0)
            {
                // extract only the filename
                var fileName = Path.GetFileName(files.FileName);
                // store the file inside ~/App_Data/uploads folder
                path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                files.SaveAs(path);              
            }

            //parsen hier. 
            //if (files.ContentType.Equals("application/pdf"))
            //if (files.FileName.EndsWith(".pdf"))
            string filename = files.FileName;
            string fileExtension = filename.Substring(filename.Length - 4);
            DbContext dbx = new DroneDBEntities();
            Creator c = new Creator(dbx);

            c.GetParser(fileExtension, path, 1);
            //c.GetParser(".dat", path, 1); //dat testing 

            System.Diagnostics.Debug.WriteLine("net voor de return");
            return View(); //gwn op zelfde pagina blijven

            // onderstaande code zorgde voor 403.14
            // redirect back to the index action to show the form once again
            //return RedirectToAction("Index");
        }

        //public ActionResult Index(HttpPostedFileBase file)
        //{

        //    if (file != null && file.ContentLength > 0)
        //        try
        //        {
        //            string path = Path.Combine(Server.MapPath("~/App_Data"),
        //                                       Path.GetFileName(file.FileName));
        //            file.SaveAs(path);
        //            ViewBag.Message = "File uploaded successfully";
        //        }
        //        catch (Exception ex)
        //        {
        //            ViewBag.Message = "ERROR:" + ex.Message.ToString();
        //        }
        //    else
        //    {
        //        ViewBag.Message = "You have not specified a file.";
        //    }





        //    return View();
        //}


        //    protected void UploadButton_Click(object sender, EventArgs e)
        //    {

        //        if (FileUploadControl.HasFile)
        //        {
        //            try
        //            {
        //                string filename = Path.GetFileName(FileUploadControl.FileName);
        //                FileUploadControl.SaveAs(Server.MapPath("~/") + filename);
        //                StatusLabel.Text = "Upload status: File uploaded!";
        //            }
        //            catch (Exception ex)
        //            {
        //                StatusLabel.Text = "Upload status: The file could not be uploaded. The following error occured: " + ex.Message;
        //            }
        //        }
        //    }
    }
}