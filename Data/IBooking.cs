using Biluthyrning.Models;

namespace Biluthyrning.Data
{
    public interface IBooking
    {
        public Task<Booking> GetByIdAsync(int id);
        public Task<IEnumerable<Booking>> GetAllAsync();
        public Task AddAsync(Booking booking);
        public Task UpdateAsync(Booking booking);
        public Task DeleteAsync(int id);
    }
}
