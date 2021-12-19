using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shop.Models
{
    public class SpecialTag
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string Tag { get; set; }
    }
}
