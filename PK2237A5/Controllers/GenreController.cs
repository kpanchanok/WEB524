using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PK2237A5.Controllers
{
    public class GenreController : Controller
    {
        public Manager m = new Manager();

        // GET: Genre
        public ActionResult Index()
        {
            var genre = m.GenreGetAll();
            return View(genre);
        }

        
    }
}
