using Biluthyrning.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Biluthyrning.ViewModels
{
    public class ConfirmBookingVM
    {
        public int Id { get; set; } 
        [Required]
        [Display(Name = "Car ID")]
        public int CarId { get; set; }
        [Display(Name = "Car model")]
        public string CarModel { get; set; } = "";
        [Display(Name = "Car brand")]

        public string CarBrand { get; set; } = "";
        [Required]
        [Display(Name = "Start date")]

        public DateTime StartDate { get; set; } = DateTime.Today;
        [Required]
        [Display(Name = "Return date")]

        public DateTime EndDate { get; set; } = DateTime.Today.AddDays(1);
        [Display(Name = "Username")]

        public string UserName { get; set; } = "";
        [Required]
        [Display(Name = "User ID")]

        public int UserId { get; set; }
        [Display(Name = "Car Category")]

        public string CarCategoryName { get; set; } = "";
        [Display(Name = "Car Category ID")]

        public int CarCategoryId { get; set; }
        public decimal Price { get; set; }
        [Display(Name = "Total cost")]

        public decimal TotalCost { get; set; }
        public List<Car> Cars { get; set; } = new();
    }
}
