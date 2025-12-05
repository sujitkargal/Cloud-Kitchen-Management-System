using Spice_Cloud.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Spice_Cloud.Models
{
    public class Dish
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string DishName { get; set; }

        [Required, StringLength(50)]
        public string Category { get; set; }

        [Required, Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Range(0, 100)]
        public int Discount { get; set; }
        public string ImagePath { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }

        [StringLength(500)]
        public string Description { get; set; }
        public int? ChefId { get; set; }
        public Chef Chef { get; set; }

    }
}
