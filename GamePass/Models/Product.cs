using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GamePass.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public string Publisher { get; set; }

        [Required]
        [Range(1, 10000)]
        public double Price { get; set; }

        public string ImageUrl { get; set; }

        // foreign key relationship
        [Required]
        public int GenreId { get; set; }
        [ForeignKey("GenreId")]
        public Genre Genre { get; set; }

        [Required]
        public int PlatformId { get; set; }
        [ForeignKey("PlatformId")]
        public Platform Platform { get; set; }

    }
}
