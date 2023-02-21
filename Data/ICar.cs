using Biluthyrning.Models;

namespace Biluthyrning.Data
{
    public interface ICar
    {
        Car GetById(int id); 
         IEnumerable<Car> GetAll();
        void Create (Car car);
        void Update (Car car);
        void Delete (Car car);

    }
}
