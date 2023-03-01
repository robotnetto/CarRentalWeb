using Biluthyrning.Models;
using Microsoft.EntityFrameworkCore;

namespace Biluthyrning.Data
{
    public class CarRepository : ICar
    {
        private readonly CarRentalContext carContext;

        public CarRepository(CarRentalContext carContext)
        {
            this.carContext = carContext;
        }
       public async Task CreateAsync(Car car)
        {
           carContext.Cars.Add(car);
            await carContext.SaveChangesAsync();
        }

       public async Task DeleteAsync(Car car)
        {
            carContext.Cars.Remove(car);
             await carContext.SaveChangesAsync();
        }

        public async Task <IEnumerable<Car>> GetAllAsync()
        {
            return await carContext.Cars.OrderBy(c => c.CarId).ToListAsync();
        }

        public async Task <Car> GetByIdAsync(int id)
        {
            return await carContext.Cars.FirstOrDefaultAsync(c => c.CarId == id);
        }

       public async Task UpdateAsync(Car car)
        {
            carContext.Update(car);
           await carContext.SaveChangesAsync();
        }
        public async Task<IEnumerable<Car>> SearchCarAsync(string search)
        {
            return await carContext.Cars.Where
                (c => c.Model.Contains(search) || c.Brand.Contains(search)).ToListAsync();
        }

    }
}
