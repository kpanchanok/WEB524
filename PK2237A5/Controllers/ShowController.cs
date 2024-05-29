using PK2237A5.Data;
using PK2237A5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PK2237A5.Controllers
{
    public class ShowController : Controller
    {
        public Manager m = new Manager();

        // GET: Show
        public ActionResult Index()
        {
            var show = m.ShowGetAll();
            return View(show);
        }

        // GET: Show/Details/5
        public ActionResult Details(int? id)
        {
            var show = m.ShowGetByIdWithDetail(id.GetValueOrDefault());

            if (show == null)
            {
                return HttpNotFound();
            }
            else
            {
                // Pass the object to the view
                return View(show);
            }
        }
        [Authorize(Roles = "Clerk")]
        [Route("Show/{id}/AddEpisode")]
        [HttpGet]

        public ActionResult AddEpisode(int? id)
        {

            var s = m.ShowGetByIdWithDetail(id.GetValueOrDefault());

            if (s == null)
            {
                return HttpNotFound();
            }

            else
            {

                var e = new EpisodeAddFormViewModel();
                e.Clerk = User.Identity.Name;
                e.ShowId = s.Id;
                e.ShowName = s.Name;

                e.GenresList = new SelectList(m.GenreGetAll(), "Name", "Name");

                return View(e);
            }
        }

        [Authorize(Roles = "Clerk")]
        [HttpPost]
        [Route("Show/{id}/AddEpisode")]
        [ValidateInput(false)]
        public ActionResult AddEpisode(EpisodeAddViewModel newItem)
        {

            if (!ModelState.IsValid)
            {
                return View(newItem);
            }

            // Process the input
            var addedItem = m.EpisodeAdd(newItem);

            if (addedItem == null)
            {
                return View(newItem);
            }
            else
            {
                return RedirectToAction("Details", "Episode", new { id = addedItem.Id });
            }
        }

    }
}
