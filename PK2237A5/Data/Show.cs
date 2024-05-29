using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PK2237A5.Data
{
    public class Show
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Genre { get; set; }

        [Required]
        public DateTime ReleaseDate { get; set; } = DateTime.Now;

        [Required]
        [StringLength(250)]
        public string ImageUrl { get; set; }

        [Required]
        [StringLength(250)]
        public string Coordinator { get; set; }

        //Navigation
        public ICollection<Actor> Actors { get; set; }
        public ICollection<Episode> Episodes { get; set; }

    }
}