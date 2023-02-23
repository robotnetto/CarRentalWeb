using Biluthyrning.Models;

namespace Biluthyrning.ViewModels
{
    public class UserVM
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = "";
        public List<Booking> Bookings { get; set; } = new();

    }
}
