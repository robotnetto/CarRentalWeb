using Biluthyrning.Models;

namespace Biluthyrning.Data
{
    public interface ICar
    {
       Task <Car> GetByIdAsync(int id); 
       Task <IEnumerable<Car>> GetAllAsync();
        Task CreateAsync (Car car);
        Task UpdateAsync (Car car);
        Task DeleteAsync (Car car);

    }
}
