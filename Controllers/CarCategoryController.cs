using Biluthyrning.Data;
using Biluthyrning.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Biluthyrning.Controllers
{
    public class CarCategoryController : Controller
    {
        private readonly ICarCategory carCategoryRepo;
        private readonly ICar carRepo;

        public CarCategoryController(ICarCategory carCategoryRepo, ICar carRepo)
        {
            this.carCategoryRepo = carCategoryRepo;
            this.carRepo = carRepo;
        }

        // GET: CarCategoryController
        public ActionResult Index()
        {
            return View(carCategoryRepo.GetAll());
        }

        // GET: CarCategoryController/Details/5
        public ActionResult Details(int id)
        {

            return View(carCategoryRepo.GetById(id));
        }

        // GET: CarCategoryController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CarCategoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CarCategory carCategory)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    carCategoryRepo.Create(carCategory);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return View();
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: CarCategoryController/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == null || carCategoryRepo.GetAll() == null)
            {
                return NotFound();
            }

            var carCategory = carCategoryRepo.GetById(id);
            if(carCategory == null)
            {
                return NotFound();
            }
            return View(carCategory);
        }

        // POST: CarCategoryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, CarCategory carCategory)
        {
            if(id != carCategory.Id)
            {
                return NotFound();
            }
            if(ModelState.IsValid)
            {
                try
                {
                    carCategoryRepo.Update(carCategory);
                }
                catch(Exception)
                {
                    return View();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(carCategory);
        }

        // GET: CarCategoryController/Delete/5
        public ActionResult Delete(int id)
        {
            if(id == null || carCategoryRepo.GetAll() == null)
            {
                return NotFound();
            }

            var carCategory = carCategoryRepo.GetById(id);
            if(carCategory == null)
            {
                return NotFound();
            }
            return View();
        }

        // POST: CarCategoryController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var carCategory = carCategoryRepo.GetById(id);
            if(ModelState.IsValid)
            {
                try
                {
                    carCategoryRepo.Delete(carCategory);
                }
                catch (Exception)
                {
                    return View();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(carCategory);
        }
    }
}