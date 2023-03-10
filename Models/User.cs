using System.ComponentModel.DataAnnotations;

namespace Biluthyrning.Models
{
    public class User
    {
        public int UserId { get; set; }
        [Required, MinLength(2), MaxLength(15)]
        public string UserName { get; set; } = "";
        [Required, MinLength(8), MaxLength(20)]
        public string Password { get; set; } = "";
        public bool IsAdmin { get; set; } = false;
        [Required]
        public string Email { get; set; } = "";
        [Required]
        public int PhoneNr { get; set; }
        public List<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
