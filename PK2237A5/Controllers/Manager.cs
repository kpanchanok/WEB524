using AutoMapper;
using PK2237A5.Data;
using PK2237A5.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Security.Claims;
using System.Web;

// ************************************************************************************
// WEB524 Project Template V2 == 2237-92ac95cd-9559-4079-8c98-bd8351374cc7
//
// By submitting this assignment you agree to the following statement.
// I declare that this assignment is my own work in accordance with the Seneca Academic
// Policy. No part of this assignment has been copied manually or electronically from
// any other source (including web sites) or distributed to other students.
// ************************************************************************************

namespace PK2237A5.Controllers
{
    public class Manager
    {

        // Reference to the data context
        private ApplicationDbContext ds = new ApplicationDbContext();

        // AutoMapper instance
        public IMapper mapper;

        // Request user property...

        // Backing field for the property
        private RequestUser _user;

        // Getter only, no setter
        public RequestUser User
        {
            get
            {
                // On first use, it will be null, so set its value
                if (_user == null)
                {
                    _user = new RequestUser(HttpContext.Current.User as ClaimsPrincipal);
                }
                return _user;
            }
        }

        // Default constructor...
        public Manager()
        {
            // If necessary, add constructor code here

            // Configure the AutoMapper components
            var config = new MapperConfiguration(cfg =>
            {
                // Define the mappings below, for example...
                // cfg.CreateMap<SourceType, DestinationType>();
                // cfg.CreateMap<Product, ProductBaseViewModel>();

                cfg.CreateMap<Models.RegisterViewModel, Models.RegisterViewModelForm>();

                cfg.CreateMap<Genre, GenreBaseViewModel>();

                cfg.CreateMap<Actor, ActorBaseViewModel>();
                cfg.CreateMap<Actor, ActorWithShowInfoViewModel>();
                cfg.CreateMap<ActorAddViewModel, Actor>();

                cfg.CreateMap<Show, ShowBaseViewModel>();
                cfg.CreateMap<Show, ShowWithInfoViewModel>();
                cfg.CreateMap<ShowAddViewModel, Show>();

                cfg.CreateMap<Episode, EpisodeBaseViewModel>();
                cfg.CreateMap<Episode, EpisodeWithShowViewModel>();
                cfg.CreateMap<Episode, EpisodeWithShowNameViewModel>();
                cfg.CreateMap<EpisodeAddViewModel, Show>();
            });

            mapper = config.CreateMapper();

            // Turn off the Entity Framework (EF) proxy creation features
            // We do NOT want the EF to track changes - we'll do that ourselves
            ds.Configuration.ProxyCreationEnabled = false;

            // Also, turn off lazy loading...
            // We want to retain control over fetching related objects
            ds.Configuration.LazyLoadingEnabled = false;
        }


        // Add your methods below and call them from controllers. Ensure that your methods accept
        // and deliver ONLY view model objects and collections. When working with collections, the
        // return type is almost always IEnumerable<T>.
        //
        // Remember to use the suggested naming convention, for example:
        // ProductGetAll(), ProductGetById(), ProductAdd(), ProductEdit(), and ProductDelete().


        //Genre
        public IEnumerable<GenreBaseViewModel> GenreGetAll()
        {
            var genres = ds.Genres.OrderBy(g => g.Name);

            return mapper.Map<IEnumerable<Genre>, IEnumerable<GenreBaseViewModel>>(genres);
        }

        //Actor
        public IEnumerable<ActorBaseViewModel> ActorGetAll()
        {
            var actors = ds.Actors.OrderBy(a => a.Name);

            return mapper.Map<IEnumerable<Actor>, IEnumerable<ActorBaseViewModel>>(actors);
        }

        public ActorBaseViewModel ActorGetById(int id)
        {
            var actor = ds.Actors.Find(id);
            return (actor == null) ? null : mapper.Map<Actor, ActorBaseViewModel>(actor);
        }

        public ActorWithShowInfoViewModel ActorGetByIdWithDetail(int id)
        {
            var viewActor = ds.Actors.Include("Shows")
                .SingleOrDefault(a => a.Id == id);

            return mapper.Map<Actor, ActorWithShowInfoViewModel>(viewActor);
        }
        public ActorBaseViewModel ActorAdd(ActorAddViewModel newItem)
        {
            
            newItem.Executive = HttpContext.Current.User.Identity.Name;
            var addedItem = ds.Actors.Add(mapper.Map<ActorAddViewModel, Actor>(newItem));
            ds.SaveChanges();
            return (addedItem == null) ? null : mapper.Map<Actor, ActorBaseViewModel>(addedItem);
        }

        //Show
        public IEnumerable<ShowBaseViewModel> ShowGetAll()
        {
            var shows = ds.Shows.OrderBy(s => s.Name);

            return mapper.Map<IEnumerable<Show>, IEnumerable<ShowBaseViewModel>>(shows);
        }
        public ShowWithInfoViewModel ShowGetByIdWithDetail(int id)
        {
            var viewShow = ds.Shows.Include("Episodes").Include("Actors")
                        .SingleOrDefault(s => s.Id == id);

            return mapper.Map<Show, ShowWithInfoViewModel>(viewShow);
        }
        public ShowWithInfoViewModel ShowAdd(ShowAddViewModel newItem)
        {

            var a = ds.Actors.Find(newItem.ActorId);

            if (a == null)
            {
                return null;
            }
            else
            {
                var addedItem = ds.Shows.Add(mapper.Map<ShowAddViewModel, Show>(newItem));
                addedItem.Coordinator = HttpContext.Current.User.Identity.Name;
                addedItem.Actors = new List<Actor> { a };

                ds.Shows.Add(addedItem);
                ds.SaveChanges();

                return (addedItem == null) ? null : mapper.Map<Show, ShowWithInfoViewModel>(addedItem);
            }
        }


        //Episode
        public IEnumerable<EpisodeWithShowViewModel> GetAllEpisodesWithShowNames()
        {
            var episodes = ds.Episodes
                .Include("Show")
                .OrderBy(e => e.Show.Name)
                .ThenBy(e => e.SeasonNumber)
                .ThenBy(e => e.EpisodeNumber)
                .ToList();

            return mapper.Map<IEnumerable<EpisodeWithShowViewModel>>(episodes);
        }

        public EpisodeWithShowNameViewModel EpisodeGetByIdWithDetail(int id)
        {
            var viewEpisode = ds.Episodes.Include("Show")
                .SingleOrDefault(e => e.Id == id);

            return mapper.Map<Episode, EpisodeWithShowNameViewModel>(viewEpisode);
        }

        public EpisodeWithShowNameViewModel EpisodeAdd(EpisodeAddViewModel newItem)
        {
            var addedItem = ds.Episodes.Add(mapper.Map<EpisodeAddViewModel, Episode>(newItem));
            addedItem.Clerk = HttpContext.Current.User.Identity.Name;
            ds.SaveChanges();

            return (addedItem == null) ? null : mapper.Map<Episode, EpisodeWithShowNameViewModel>(addedItem);
            
        }










        // *** Add your methods ABOVE this line **

        #region Role Claims

        public List<string> RoleClaimGetAllStrings()
        {
            return ds.RoleClaims.OrderBy(r => r.Name).Select(r => r.Name).ToList();
        }

        #endregion

        #region Load Data Methods

        // Add some programmatically-generated objects to the data store
        // You can write one method or many methods but remember to
        // check for existing data first.  You will call this/these method(s)
        // from a controller action.

        public bool LoadData()
        {
            // User name
            var user = HttpContext.Current.User.Identity.Name;

            // Monitor the progress
            bool done = false;

            

            // You may load additional entities here, or you may 
            // choose to create a new method altogether.

            return done;
        }

        public bool LoadRoles()
        {
            if (ds.RoleClaims.Count() == 0)
            {
                ds.RoleClaims.Add(new RoleClaim { Name = "Admin" });
                ds.RoleClaims.Add(new RoleClaim { Name = "Executive" });
                ds.RoleClaims.Add(new RoleClaim { Name = "Coordinator" });
                ds.RoleClaims.Add(new RoleClaim { Name = "Clerk" });
            }
            
            ds.SaveChanges();

            return true;
        }

        public bool LoadGenres()
        {
            if (ds.Genres.Count() > 0) { return false; }

            ds.Genres.Add(new Genre { Name = "Action" });
            ds.Genres.Add(new Genre { Name = "Adventure" });
            ds.Genres.Add(new Genre { Name = "Comedy" });
            ds.Genres.Add(new Genre { Name = "Drama" });
            ds.Genres.Add(new Genre { Name = "Horror" });
            ds.Genres.Add(new Genre { Name = "Musical" });
            ds.Genres.Add(new Genre { Name = "Romance" });
            ds.Genres.Add(new Genre { Name = "Sci-Fi" });
            ds.Genres.Add(new Genre { Name = "Sport" });
            ds.Genres.Add(new Genre { Name = "Thriller" });

            ds.SaveChanges();

            return true;
        }

        public bool LoadActors()
        {
            if (ds.Actors.Count() > 0) { return false; }
            if (ds.Actors.Count() == 0)
            {
                ds.Actors.Add(new Actor
                {
                    Name = "Matthew Langford Perry",
                    AlternateName = "Matthew Perry",
                    BirthDate = new DateTime(1969, 8, 19),
                    Height = 1.83m,
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/c/cd/Matthew_Perry_in_support_of_Awareness_on_Drug_Courts_and_Reduced_Substance_Abuse.jpg",
                    Executive = User.Name
                });

                ds.Actors.Add(new Actor
                {
                    Name = "Emily Jean Stone",
                    AlternateName = "Emma Stone",
                    BirthDate = new DateTime(1988, 11, 6),
                    Height = 1.68m,
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/3/31/Emma_Stone_at_Maniac_UK_premiere_%28cropped%29.jpg",
                    Executive = User.Name
                });

                ds.Actors.Add(new Actor
                {
                    Name = "Mackenyu Maeda",
                    AlternateName = "",
                    BirthDate = new DateTime(1996, 11, 16),
                    Height = 1.78m,
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/5/53/SS_2023-09-20_at_10.20.26_AM.png",
                    Executive = User.Name
                });

            }
            
            ds.SaveChanges();
            return true;
        }

        public bool LoadShows()
        {
            if (ds.Shows.Count() > 0) { return false; }

            var matthewPerry = ds.Actors.SingleOrDefault(a => a.Name == "Matthew Langford Perry");

            if (matthewPerry == null) { return false; }

            if (ds.Shows.Count() == 0)
            {
                ds.Shows.Add(new Show
                {
                    Name = "Friends",
                    Genre = "Comedy",
                    ReleaseDate = new DateTime(1994, 9, 22),
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/b/bc/Friends_logo.svg",
                    Coordinator = User.Name,
                    Actors = new Actor[] { matthewPerry }

                });

                ds.Shows.Add(new Show
                {
                    Name = "The West Wing",
                    Genre = "Drama",
                    ReleaseDate = new DateTime(1999, 9, 21),
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/en/5/56/TheWestWing.JPG",
                    Coordinator = User.Name,
                    Actors = new Actor[] { matthewPerry }
                });

            }

            ds.SaveChanges();
            
            return true;
        }

        public bool LoadEpisodes()
        {

            if (ds.Episodes.Count() > 0) { return false; }

            var friendsShow = ds.Shows.SingleOrDefault(s => s.Name == "Friends");
            if (friendsShow== null) { return false; }

            var westWingShow = ds.Shows.SingleOrDefault(s => s.Name == "The West Wing");
            if (westWingShow == null) { return false; }

            ds.Episodes.Add(new Episode
            {
                Name = "The One Where Monica Gets a Roommate",
                SeasonNumber = 1,
                EpisodeNumber = 1,
                Genre = "Comedy",
                AirDate = new DateTime(1994, 9, 22),
                ImageUrl = "https://images5.fanpop.com/image/photos/28000000/Friends-Episode-I-The-One-Where-Monica-Gets-a-Roommate-jennifer-aniston-28065561-600-337.jpg",
                Clerk = User.Name,
                Show = friendsShow
            });
            ds.Episodes.Add(new Episode
            {
                Name = "The One with All the Thanksgivings",
                SeasonNumber = 5,
                EpisodeNumber = 8,
                Genre = "Comedy",
                AirDate = new DateTime(1998, 11, 19),
                ImageUrl = "https://cookiesandsangria.files.wordpress.com/2013/11/friends-t-gives.jpg",
                Clerk = User.Name,
                Show = friendsShow
            });

            ds.Episodes.Add(new Episode
            {
                Name = "The Last One Part 1",
                SeasonNumber = 10,
                EpisodeNumber = 17,
                Genre = "Comedy",
                AirDate = new DateTime(2004, 5, 6),
                ImageUrl = "https://static.tvmaze.com/uploads/images/large_landscape/197/492535.jpg",
                Clerk = User.Name,
                Show = friendsShow
            });


            ds.Episodes.Add(new Episode
            {
                Name = "Pilot - A Proportional Response",
                SeasonNumber = 1,
                EpisodeNumber = 1,
                Genre = "Drama",
                AirDate = new DateTime(1999, 9, 22),
                ImageUrl = "https://criticallytouched.files.wordpress.com/2017/03/1x03_top6.png",
                Clerk = User.Name,
                Show = westWingShow
            });
            ds.Episodes.Add(new Episode
            {
                Name = "Post Hoc, Ergo Propter Hoc",
                SeasonNumber = 1,
                EpisodeNumber = 2,
                Genre = "Drama",
                AirDate = new DateTime(1999, 9, 29),
                ImageUrl = "https://3.bp.blogspot.com/-M6x3tsTq3vc/UQAw8dysjbI/AAAAAAAACDw/FdaZx35Q1y8/s1600/215341.jpg",
                Clerk = User.Name,
                Show = westWingShow
            });
            ds.Episodes.Add(new Episode
            {
                Name = "A Proportional Response",
                SeasonNumber = 1,
                EpisodeNumber = 3,
                Genre = "Drama",
                AirDate = new DateTime(1999, 10, 6),
                ImageUrl = "https://tv-fanatic-res.cloudinary.com/iu/s--BlHpSR1s--/t_full/cs_srgb,f_auto,fl_strip_profile.lossy,q_auto:420/v1505764380/a-new-body-man-the-west-wing-s1e3.png",
                Clerk = User.Name,
                Show = westWingShow
            });

            ds.SaveChanges();

            return true;
        }

        public bool RemoveShow()
        {
            try
            {
                foreach (var e in ds.Shows)
                {
                    ds.Entry(e).State = System.Data.Entity.EntityState.Deleted;
                }
                ds.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool RemoveEpisode()
        {
            try
            {
                foreach (var e in ds.Episodes)
                {
                    ds.Entry(e).State = System.Data.Entity.EntityState.Deleted;
                }
                ds.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool RemoveActor()
        {
            try
            {
                foreach (var e in ds.Actors)
                {
                    ds.Entry(e).State = System.Data.Entity.EntityState.Deleted;
                }
                ds.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        } 
        public bool RemoveData()
        {
            try
            {
                foreach (var e in ds.RoleClaims)
                {
                    ds.Entry(e).State = System.Data.Entity.EntityState.Deleted;
                }
                ds.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool RemoveDatabase()
        {
            try
            {
                return ds.Database.Delete();
            }
            catch (Exception)
            {
                return false;
            }
        }

    }

    #endregion

    #region RequestUser Class

    // This "RequestUser" class includes many convenient members that make it
    // easier work with the authenticated user and render user account info.
    // Study the properties and methods, and think about how you could use this class.

    // How to use...
    // In the Manager class, declare a new property named User:
    //    public RequestUser User { get; private set; }

    // Then in the constructor of the Manager class, initialize its value:
    //    User = new RequestUser(HttpContext.Current.User as ClaimsPrincipal);

    public class RequestUser
    {
        // Constructor, pass in the security principal
        public RequestUser(ClaimsPrincipal user)
        {
            if (HttpContext.Current.Request.IsAuthenticated)
            {
                Principal = user;

                // Extract the role claims
                RoleClaims = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);

                // User name
                Name = user.Identity.Name;

                // Extract the given name(s); if null or empty, then set an initial value
                string gn = user.Claims.SingleOrDefault(c => c.Type == ClaimTypes.GivenName).Value;
                if (string.IsNullOrEmpty(gn)) { gn = "(empty given name)"; }
                GivenName = gn;

                // Extract the surname; if null or empty, then set an initial value
                string sn = user.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Surname).Value;
                if (string.IsNullOrEmpty(sn)) { sn = "(empty surname)"; }
                Surname = sn;

                IsAuthenticated = true;
                // You can change the string value in your app to match your app domain logic
                IsAdmin = user.HasClaim(ClaimTypes.Role, "Admin") ? true : false;
            }
            else
            {
                RoleClaims = new List<string>();
                Name = "anonymous";
                GivenName = "Unauthenticated";
                Surname = "Anonymous";
                IsAuthenticated = false;
                IsAdmin = false;
            }

            // Compose the nicely-formatted full names
            NamesFirstLast = $"{GivenName} {Surname}";
            NamesLastFirst = $"{Surname}, {GivenName}";
        }

        // Public properties
        public ClaimsPrincipal Principal { get; private set; }

        public IEnumerable<string> RoleClaims { get; private set; }

        public string Name { get; set; }

        public string GivenName { get; private set; }

        public string Surname { get; private set; }

        public string NamesFirstLast { get; private set; }

        public string NamesLastFirst { get; private set; }

        public bool IsAuthenticated { get; private set; }

        public bool IsAdmin { get; private set; }

        public bool HasRoleClaim(string value)
        {
            if (!IsAuthenticated) { return false; }
            return Principal.HasClaim(ClaimTypes.Role, value) ? true : false;
        }

        public bool HasClaim(string type, string value)
        {
            if (!IsAuthenticated) { return false; }
            return Principal.HasClaim(type, value) ? true : false;
        }
    }

    #endregion

}