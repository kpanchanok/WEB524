using PK2237A5.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PK2237A5.Models
{
    public class EpisodeBaseViewModel
    {

        public int Id { get; set; }

        public string Name { get; set; }

        [Display(Name = "Season")]
        public int SeasonNumber { get; set; }

        [Display(Name = "Episode")]
        public int EpisodeNumber { get; set; }

        public string Genre { get; set; }

        [Display(Name = "Date Aired")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime AirDate { get; set; }

        [Display(Name = "Image")]
        public string ImageUrl { get; set; }

        public string Clerk { get; set; }

    }
}