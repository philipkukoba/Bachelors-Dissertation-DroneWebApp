using DroneWebApp.Models;
using DroneWebApp.Models.SimpleFactoryPattern;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DroneWebApp.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View("Index");
        }

        [AllowAnonymous]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View("About");
        }

        [AllowAnonymous]
        public ActionResult Contact()
        {
            ViewBag.Message = "Please contact us:";

            return View("Contact");
        }
    }
}