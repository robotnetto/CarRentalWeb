using Biluthyrning.Models;
using System.Runtime.InteropServices;

namespace Biluthyrning.Data
{
    public interface IBooking
    {
        public Booking GetById(int id);
        public IEnumerable<Booking> GetAll();  
        public void Create(Booking booking);
        public void Update(Booking booking);
        public void Delete(int id);
    }
}
