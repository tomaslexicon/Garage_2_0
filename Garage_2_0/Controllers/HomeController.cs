using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Garage_2_0.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Our vision";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Our professional support team is always ready to answer your questions";

            return View();
        }
    }
}