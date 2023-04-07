using Biluthyrning.Models;
using Microsoft.EntityFrameworkCore;

namespace Biluthyrning.Data
{
    public class BookingRepository : IBooking
    {
        private readonly CarRentalContext context;

        public BookingRepository(CarRentalContext context)
        {
            this.context = context;
        }
        public async Task AddAsync(Booking booking)
        {
            context.Bookings.Add(booking);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var booking = context.Bookings.FirstOrDefault(b => b.Id == id);
            if(booking != null)
            {
                context.Bookings.Remove(booking);
                await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Booking>> GetAllAsync()
        {
            return await context.Bookings.OrderByDescending(b => b.StartDate).ToListAsync();
        }

        public async Task<Booking> GetByIdAsync(int id)
        {
            return await context.Bookings.FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task UpdateAsync(Booking booking)
        {
            context.Bookings.Update(booking);
            await context.SaveChangesAsync();
        }
    }
}
