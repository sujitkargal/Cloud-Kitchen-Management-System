using System.ComponentModel.DataAnnotations;
namespace Spice_Cloud.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Chef
    {
        [Key]
        public int ChefId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Mobile { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string Area { get; set; }

        [Required]
        public string Speciality { get; set; }

        [Required]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
    }


}
