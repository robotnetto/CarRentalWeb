using Biluthyrning.Models;

namespace Biluthyrning.Data
{
    public interface ICarCategory
    {
        public IEnumerable<CarCategory> GetAll();
        public CarCategory GetById(int id);
        public void Create(CarCategory carCategory);
        public void Update(CarCategory carCategory);
        public void Delete(CarCategory carCategory);
    }
}