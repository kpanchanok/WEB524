using Antlr.Runtime.Misc;
using PK2237A5.Data;
using PK2237A5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PK2237A5.Controllers
{
    public class ActorController : Controller
    {
        public Manager m = new Manager();

        // GET: Actor
        public ActionResult Index()
        {
            var actor = m.ActorGetAll();
            return View(actor);
        }

        // GET: Actor/Details/5
        public ActionResult Details(int? id)
        {

            var actor = m.ActorGetByIdWithDetail(id.GetValueOrDefault());
            if (actor == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(actor);
            }

        }

        // GET: Actor/Create
        [Authorize(Roles = "Executive")]
        public ActionResult Create()
        {
            var actor = new ActorAddViewModel();

            actor.Executive = User.Identity.Name;
            return View(actor);
        }

        // POST: Actor/Create
        [HttpPost]
        public ActionResult Create(ActorAddViewModel newItem)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(newItem);
                }

                var addedItem = m.ActorAdd(newItem);

                if (addedItem == null)
                {
                    return View(newItem);
                }

                else
                {
                    return RedirectToAction("Details", new { id = addedItem.Id });
                }
            }
            catch
            {
                return View();
            }
        }


        [Authorize(Roles = "Coordinator")]
        [Route("Actor/{id}/AddShow")]
        [HttpGet]
        
        public ActionResult AddShow(int? id)
        {

            var a = m.ActorGetById(id.GetValueOrDefault());

            if (a == null)
            {
                return HttpNotFound();
            }

            else
            {
               
                var o = new ShowAddFormViewModel();
                o.Coordinator = User.Identity.Name;
                o.ActorId = a.Id;
                o.ActorsName = a.Name;

                o.GenresList = new SelectList(m.GenreGetAll(), "Name", "Name");

                var selectedActor = new List<int> { a.Id };
                o.ActorsList = new MultiSelectList(m.ActorGetAll(), "Id", "Name", selectedActor);

                return View(o);
            }
        }

        [Authorize(Roles = "Coordinator")]
        [HttpPost]
        [Route("Actor/{id}/AddShow")]
        [ValidateInput(false)]
        public ActionResult AddShow(ShowAddViewModel newItem)
        {
            
            if (!ModelState.IsValid)
            {
                return View(newItem);
            }

            var addedItem = m.ShowAdd(newItem);

            if (addedItem == null)
            {
                return View(newItem);
            }
            else
            {
                return RedirectToAction("Details", "Show", new { id = addedItem.Id });
            }
        }



    }
}
