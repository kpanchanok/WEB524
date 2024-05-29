using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PK2237A5.Models
{
    public class ActorAddViewModel
    {

        [Required]
        [StringLength(150)]
        public string Name { get; set; }

        [Display(Name = "Altername Name")]
        [StringLength(150)]
        public string AlternateName { get; set; }

        [Display(Name = "Birth date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true, NullDisplayText = "yyyy-MM-dd")]
        public DateTime BirthDate { get; set; }

        [Display(Name = "Height (m)")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal Height { get; set; }

        [Display(Name = "Image")]
        public string ImageUrl { get; set; }

        public string Executive { get; set; }

    }
}