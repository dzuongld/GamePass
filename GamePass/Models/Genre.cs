using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GamePass.Models
{
    public class Genre
    {
        public int Id { get; set; }

        [Display(Name = "Genre")]
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
