using Microsoft.Build.Framework;

namespace Biluthyrning.Models
{
    public class Car
    {
        public int CarId { get; set; }
        [Required]
        public string Model { get; set; } = "";
        [Required]
        public string Brand { get; set; } = "";
        [Required]
        public string Color { get; set; } = "";
        [Required]
        public int CarCategoryId { get; set; }

    }
}
