using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Spice_Cloud.Models
{
    public class Admin
    {
        public int AdminId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [NotMapped]
        public string ConfirmPassword { get; set; }
    }
}
