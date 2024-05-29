using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PK2237A5.Controllers
{
    public class HomeController : Controller
    {
        Manager m = new Manager();
        public ActionResult Index()
        {
            // Load the role claims
            m.LoadData();

            return View();
        }
    }
}