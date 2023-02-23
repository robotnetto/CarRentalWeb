using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Biluthyrning.Data;
using Biluthyrning.Models;

namespace Biluthyrning.Controllers
{
    public class CarsController : Controller
    {
        private readonly ICar carRepo;
        private readonly CarRentalContext _context;

        public CarsController(ICar carRepo, CarRentalContext context)
        {
            this.carRepo = carRepo;
            this._context = context;
        }

        // GET: Cars
        public ActionResult Index()
        {
              //return carRepo != null ? 
              //            View(carRepo.GetAll()) :
              //            Problem("Entity set 'CarRentalContext.Cars'  is null.");
              return View(carRepo.GetAll());
        }

        // GET: Cars/Details/5
        public IActionResult Details(int id)
        {
            if (id! == null || carRepo == null)
            {
                return NotFound();
            }

            var car = carRepo.GetById(id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        // GET: Cars/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cars/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("CarId,Model,Brand,Color,CarCategoryId")] Car car)
        {
            if (ModelState.IsValid)
            {
                carRepo.Create(car);
                return RedirectToAction(nameof(Index));
            }
            return View(car);
        }

        // GET: Cars/Edit/5
        public  IActionResult Edit(int id)
        {
            if (id! == null || carRepo == null)
            {
                return NotFound();
            }

            var car = carRepo.GetById(id);
            if (car == null)
            {
                return NotFound();
            }
            return View(car);
        }

        // POST: Cars/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("CarId,Model,Brand,Color,CarCategoryId")] Car car)
        {
            if (id != car.CarId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    carRepo.Update(car);
                }
                catch (Exception)
                {
                    return View();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(car);
        }

        // GET: Cars/Delete/5
        public IActionResult Delete(int id)
        {
            if (id == null || carRepo == null)
            {
                return NotFound();
            }

            var car = carRepo.GetById(id);
                
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        // POST: Cars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (carRepo == null)
            {
                return Problem("Entity set 'CarRentalContext.Cars'  is null.");
            }
            var car = carRepo.GetById(id);
            if (car != null)
            {
                carRepo.Delete(car);
            }
            
            return RedirectToAction(nameof(Index));
        }
    }
}
