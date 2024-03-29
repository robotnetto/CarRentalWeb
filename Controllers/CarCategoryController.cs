﻿using Biluthyrning.Data;
using Biluthyrning.Models;
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
        public async Task<IActionResult> Index(string search)
        {
            ViewBag.UserType = Request.Cookies["UserType"];
            ViewBag.SearchedCategory = search;
            if (string.IsNullOrWhiteSpace(search))
            {
                return View(await carCategoryRepo.GetAllAsync());
            }
            else
            {
                return View(await carCategoryRepo.GetSearchedAsync(search));
            }
        }

        // GET: CarCategoryController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            ViewBag.UserType = Request.Cookies["UserType"];
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
            if(id == null || carCategoryRepo == null)
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
            if( carCategoryRepo == null)
            {
                return Problem("Entity set 'CarRentalContext.CarCategory'  is null.");
            }
            var carCategory = await carCategoryRepo.GetByIdAsync(id);
            if (carCategory != null)
            {
                await carCategoryRepo.DeleteAsync(id);

            }
            return RedirectToAction(nameof(Index));
            
        }
    }
}