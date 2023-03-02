
using System.ComponentModel.DataAnnotations;

namespace Biluthyrning.Models
{
    public class Car
    {
        public int CarId { get; set; }
        [Required(ErrorMessage = "Please enter the car model.")]
        public string Model { get; set; } = "";
        [Required(ErrorMessage = "Please enter the car brand.")]
        public string Brand { get; set; } = "";
        [Required(ErrorMessage = "Please enter the car color.")]
        public string Color { get; set; } = "";
        [Required(ErrorMessage ="Please enter the amount")]
        [Display(Name = "Price/Day")]
        public decimal Amount { get; set; }
        [Required]
        public int CarCategoryId { get; set; }
    }
}
