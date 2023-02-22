using Biluthyrning.Models;

namespace Biluthyrning.Data
{
    public class CarRepository : ICar
    {
        private readonly CarRentalContext carContext;

        public CarRepository(CarRentalContext carContext)
        {
            this.carContext = carContext;
        }
       public void Create(Car car)
        {
           carContext.Cars.Add(car);
            carContext.SaveChanges();
        }

       public void Delete(Car car)
        {
            carContext.Cars.Remove(car);
            carContext.SaveChanges();
        }

        IEnumerable<Car> ICar.GetAll()
        {
            return carContext.Cars.OrderBy(c => c.CarId);
        }

        public Car GetById(int id)
        {
            return carContext.Cars.FirstOrDefault(c => c.CarId == id)!;
        }

       public void Update(Car car)
        {
            carContext.Update(car);
            carContext.SaveChanges();
        }
    }
}
