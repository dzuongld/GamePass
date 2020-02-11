using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GamePass.Models
{
    public class Platform
    {
        public int Id { get; set; }

        [Display(Name = "Platform")]
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
