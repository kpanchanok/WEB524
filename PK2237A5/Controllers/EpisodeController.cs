using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PK2237A5.Controllers
{
    public class EpisodeController : Controller
    {
        public Manager m = new Manager();

        // GET: Episode
        public ActionResult Index()
        {
            var episode = m.GetAllEpisodesWithShowNames();
            return View(episode);
        }

        // GET: Episode/Details/5
        public ActionResult Details(int? id)
        {

            var episode = m.EpisodeGetByIdWithDetail(id.GetValueOrDefault());
            if (episode == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(episode);
            }

        }

    }
}
