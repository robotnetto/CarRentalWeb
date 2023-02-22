using Biluthyrning.Models;

namespace Biluthyrning.Data
{
    public class CarCategoryRepository : ICarCategory
    {
        private readonly CarRentalContext context;

        public CarCategoryRepository(CarRentalContext context)
        {
            this.context = context;
        }

        public void Create(CarCategory carCategory)
        {
            context.Categories.Add(carCategory);
            context.SaveChanges();
        }

        public void Delete(CarCategory carCategory)
        {
            context.Categories.Remove(carCategory);
            context.SaveChanges();
        }

        public IEnumerable<CarCategory> GetAll()
        {
            return context.Categories.OrderBy(c => c.Id);
        }

        public CarCategory GetById(int id)
        {
            return context.Categories.FirstOrDefault(c => c.Id == id);
        }

        public void Update(CarCategory carCategory)
        {
            context.Categories.Update(carCategory);
            context.SaveChanges();
        }
    }
}