using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PK2237A5.Models
{
    public class EpisodeWithShowViewModel : EpisodeBaseViewModel
    {
        
        [Display(Name = "Show")]
        public string ShowName { get; set; }
    }
}