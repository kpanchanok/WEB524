using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace PK2237A5.Models
{
    public class EpisodeAddViewModel
    {
        public EpisodeAddViewModel()
        {
            AirDate = DateTime.Now;
            SeasonNumber = 1;
            EpisodeNumber = 1;
        }


        [Required]
        [StringLength(150)]
        public string Name { get; set; }

        [Display(Name = "Season")]
        [Required]
        public int SeasonNumber { get; set; }

        [Display(Name = "Episode")]
        [Required]
        public int EpisodeNumber { get; set; }

        public string Genre { get; set; }

        [Display(Name = "Date Aired")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required]
        public DateTime AirDate { get; set; }

        [Display(Name = "Image")]
        [Required]
        [StringLength(250)]
        public string ImageUrl { get; set; }

        public string Clerk { get; set; }

        public int GenreId { get; set; }

        public int ShowId { get; set; }

        public string ShowName { get; set; }
    }
}