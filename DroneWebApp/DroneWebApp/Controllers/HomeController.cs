using DroneWebApp.Models.SimpleFactoryPattern;
using DroneWebApp.Models.SimpleFactoryPattern.Parsers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DroneWebApp.Controllers
{
    public class HomeController : Controller
    {
        Creator creator = new Creator();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            // TODO: have the user enter this via the View
            string strPath = @"C:\Users\Bryan\Desktop\Dataset Bachelorproef UGent_200214\02 Harelbeke drone data\190912\02 Geotiff +xyz pointcloud\Drone Flight - Harelbeke - 190912_dsm_10cm.xyz";
            string ext = Path.GetExtension(strPath);
            creator.GetParser(ext, strPath);

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}