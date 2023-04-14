using Biluthyrning.Models;

namespace Biluthyrning.Data
{
    public interface ICar
    {
       Task <Car> GetByIdAsync(int id); 
       Task <IEnumerable<Car>> GetAllAsync();
        Task<IEnumerable<Car>> SearchCarAsync(string search); 
        Task CreateAsync (Car car);
        Task UpdateAsync (Car car);
        Task DeleteAsync (int id);

    }
}
