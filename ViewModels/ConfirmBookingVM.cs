using Biluthyrning.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Framework;

namespace Biluthyrning.ViewModels
{
    public class ConfirmBookingVM
    {
        [Required]
        public int CarId { get; set; }
        public string CarModel { get; set; } = "";
        public string CarBrand { get; set; } = "";
        [Required]
        public DateTime StartDate { get; set; } = DateTime.Now;
        [Required]
        public DateTime EndDate { get; set; } = DateTime.Now.AddDays(1);
        public string UserName { get; set; } = "";
        [Required]
        public int UserId { get; set; }
        public string CarCategoryName { get; set; } = "";
        public int CarCategoryId { get; set; }
        public decimal Price { get; set; }
        public decimal TotalCost { get; set; }
        public List<Car> Cars { get; set; } = new();
    }
}
