using Biluthyrning.Models;

namespace Biluthyrning.ViewModels
{
    public class UserVM
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = "";
        public string Password { get; set; } = "";
        public bool IsAdmin { get; set; }
        public List<BookingViewModel> Bookings { get; set; } = new();
    }
}
