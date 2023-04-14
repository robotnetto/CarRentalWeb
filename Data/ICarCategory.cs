using Biluthyrning.Models;

namespace Biluthyrning.Data
{
    public interface ICarCategory
    {
        public Task<IEnumerable<CarCategory>> GetAllAsync();
        public Task<IEnumerable<CarCategory>> GetSearchedAsync(string search);
        public Task<CarCategory> GetByIdAsync(int id);
        public Task CreateAsync(CarCategory carCategory);
        public Task UpdateAsync(CarCategory carCategory);
        public Task DeleteAsync(int id);
    }
}