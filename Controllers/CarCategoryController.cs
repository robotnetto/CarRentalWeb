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
        public async Task<IActionResult> Index()
        {
            return View( await carCategoryRepo.GetAllAsync());
        }

        // GET: CarCategoryController/Details/5
        public async Task<IActionResult> Details(int id)
        {

            return View( await carCategoryRepo.GetByIdAsync(id));
        }

        // GET: CarCategoryController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CarCategoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CarCategory carCategory)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    await carCategoryRepo.CreateAsync(carCategory);
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
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null || await carCategoryRepo.GetAllAsync() == null)
            {
                return NotFound();
            }

            var carCategory = await carCategoryRepo.GetByIdAsync(id);
            if(carCategory == null)
            {
                return NotFound();
            }
            return View(carCategory);
        }

        // POST: CarCategoryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CarCategory carCategory)
        {
            if(id != carCategory.Id)
            {
                return NotFound();
            }
            if(ModelState.IsValid)
            {
                try
                {
                    await carCategoryRepo.UpdateAsync(carCategory);
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
        public async Task<IActionResult> Delete(int id)
        {
            if(id == null || await carCategoryRepo.GetAllAsync() == null)
            {
                return NotFound();
            }

            var carCategory = await carCategoryRepo.GetByIdAsync(id);
            if(carCategory == null)
            {
                return NotFound();
            }
            return View(carCategory);
        }

        // POST: CarCategoryController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var carCategory = await carCategoryRepo.GetByIdAsync(id);
            if(ModelState.IsValid)
            {
                try
                {
                    await carCategoryRepo.DeleteAsync(carCategory);
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