using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PK2237A5.Models
{
    public class ActorWithShowInfoViewModel : ActorBaseViewModel
    {
        public IEnumerable<ShowBaseViewModel> Shows { get; set; }

        public int ShowsCount { get; set; }

    }
}