using Biluthyrning.Models;

namespace Biluthyrning.Data
{
    public class BookingRepository : IBooking
    {
        private readonly CarRentalContext context;

        public BookingRepository(CarRentalContext context)
        {
            this.context = context;
        }
        public void Create(Booking booking)
        {
            context.Bookings.Add(booking);
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            var booking = context.Bookings.FirstOrDefault(b => b.Id == id);
            context.Bookings.Remove(booking);
            context.SaveChanges();
        }

        public IEnumerable<Booking> GetAll()
        {
            return context.Bookings.OrderBy(s => s.Id);
        }

        public Booking GetById(int id)
        {
            return context.Bookings.FirstOrDefault(b => b.Id == id);
        }

        public void Update(Booking booking)
        {
            context.Bookings.Update(booking);
            context.SaveChanges();
        }
    }
}
