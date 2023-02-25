namespace Biluthyrning.ViewModels
{
    public class BookingViewModel
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public string CarModel { get; set; } = "";
        public string CarBrand { get; set; } = "";
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string UserName { get; set; } = "";
        public string CarCategoryName { get; set; } = "";
    }
}
