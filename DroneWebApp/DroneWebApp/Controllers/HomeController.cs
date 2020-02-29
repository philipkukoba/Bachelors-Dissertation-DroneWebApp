using DroneWebApp.Models;
using DroneWebApp.Models.SimpleFactoryPattern;
using DroneWebApp.Models.SimpleFactoryPattern.Parsers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DroneWebApp.Controllers
{
    public class HomeController : Controller
    {
        private Creator creator = new Creator();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            // TODO: Nu werkt de code op het activeren van de pagina About, maar later zal deze code verplaatsen naar een functie die wordt aangeroepen wanneer de gebruiker bv 'ok' klikt.
            // TODO: the user can select the file from their computer
            string strPath = @"S:\Desktop\Dataset Bachelorproef UGent_200214\02 Harelbeke drone data\190912\02 Geotiff +xyz pointcloud\Drone Flight - Harelbeke - 190912_dsm_10cm.xyz";
            string ext = Path.GetExtension(strPath);

            // TODO: have the user enter date & location via the View
            string location = "Harelbeke";
            string date = "190912";

            // Use ORM to write to Database 
            using (DroneDBEntities context = new DroneDBEntities())
            {
                DroneFlight droneFlight = new DroneFlight();
                // Check if this Primary Key already exists
                if (!context.DroneFlights.Any(flight => flight.FlightId == droneFlight.FlightId))
                {
                    context.DroneFlights.Add(droneFlight);
                    context.SaveChanges();
                }
            }

            // Parse data and write to Database
            //creator.GetParser(ext, strPath, date_and_loc.ToString()); // FIX

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Please contact us:";

            return View();
        }

        /*
        // TODO: improve this (move this?)
        public ActionResult FileUpload()
        {
            ViewBag.Message = "Upload files here.";

            return View();
        }
        */
    }
}