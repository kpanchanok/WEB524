using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PK2237A5.Models
{
    public class ShowWithInfoViewModel : ShowBaseViewModel
    {
        public List<ActorBaseViewModel> Actors { get; set; }

        public List<EpisodeBaseViewModel> Episodes { get; set; }

        public int ActorsCount { get; set; }

        public int EpisodesCount { get; set; }
    }
}