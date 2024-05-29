using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PK2237A5.Models
{
    public class ShowAddViewModel
    {
        public ShowAddViewModel()
        {
            ReleaseDate = DateTime.Now;
        }

        public string Name { get; set; }

        public string Genre { get; set; }

        [Display(Name = "Release Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ReleaseDate { get; set; }

        [Display(Name = "Image")]
        public string ImageUrl { get; set; }

        public string Coordinator { get; set; }

        public int ActorId { get; set; }

        public int GenreId { get; set; }



    }

}