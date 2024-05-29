using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PK2237A5.Data
{
    public class Actor
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; }

        [StringLength(150)]
        public string AlternateName { get; set; }

        public DateTime? BirthDate { get; set; }

        public decimal? Height { get; set; }

        [Required]
        [StringLength(250)]
        public string ImageUrl { get; set; }

        [Required]
        [StringLength(250)]
        public string Executive { get; set; }

        //Navigation
        public ICollection<Show> Shows { get; set; } = new HashSet<Show>();

    }
}