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
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            DbContext dbx = new DroneDBEntities();
            Creator c = new Creator(dbx);
            c.GetParser(".pdf", "path", 1); 

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Please contact us:";

            return View();
        }
    }
}