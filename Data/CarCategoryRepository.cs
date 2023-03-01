using Biluthyrning.Models;
using Microsoft.EntityFrameworkCore;

namespace Biluthyrning.Data
{
    public class CarCategoryRepository : ICarCategory
    {
        private readonly CarRentalContext context;

        public CarCategoryRepository(CarRentalContext context)
        {
            this.context = context;
        }

        public async Task CreateAsync(CarCategory carCategory)
        {
            context.Categories.Add(carCategory);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(CarCategory carCategory)
        {
            context.Categories.Remove(carCategory);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<CarCategory>> GetAllAsync()
        {
            return await context.Categories.OrderBy(c => c.Id).ToListAsync();
        }
        public async Task<IEnumerable<CarCategory>> GetSearchedAsync(string search)
        {
            return await context.Categories.Where(c => c.Name.Contains(search)).OrderBy(c => c.Id).ToListAsync();
        }

        public async Task<CarCategory> GetByIdAsync(int id)
        {
            return await context.Categories.Include(c => c.Cars).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task UpdateAsync(CarCategory carCategory)
        {
            context.Categories.Update(carCategory);
            await context.SaveChangesAsync();
        }
    }
}