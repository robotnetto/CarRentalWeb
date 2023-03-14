using System.ComponentModel.DataAnnotations;

namespace Biluthyrning.ViewModels
{
    public class BookingViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Car ID")]
        public int CarId { get; set; }
        [Display(Name = "Car model")]
        public string CarModel { get; set; } = "";
        [Display(Name = "Car brand")]
        public string CarBrand { get; set; } = "";
        [Required]
        [Display(Name = "Start date")]
        public DateTime StartDate { get; set; }
        [Display(Name = "Return date")]
        public DateTime EndDate { get; set; }
        [Display(Name = "Username")]
        public string UserName { get; set; } = "";
        [Display(Name = "User ID")]
        public int UserId { get; set; }
        [Display(Name = "Car Category")]
        public string CarCategoryName { get; set; } = "";
        public decimal Price { get; set;}
        [Display(Name = "Total cost")]
        public decimal TotalCost { get; set;}
    }
}
