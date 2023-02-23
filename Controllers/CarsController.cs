﻿using System;
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
        private readonly ICarCategory carCategoryRepo;

        public CarsController(ICar carRepo, ICarCategory carCategoryRepo)
        {
            this.carRepo = carRepo;
            
            this.carCategoryRepo = carCategoryRepo;
        }

        // GET: Cars
        public async Task <ActionResult> Index()
        {
              return carRepo != null ? 
                          View(await carRepo.GetAllAsync()) :
                          Problem("Entity set 'CarRentalContext.Cars'  is null.");
        }

        // GET: Cars/Details/5
        public async Task <IActionResult> Details (int id)
        {
            if (id == null || carRepo == null)
            {
                return NotFound();
            }

            var car = await carRepo.GetByIdAsync(id);
            if (car == null)
            {
                return NotFound();
            }

            return View( car);
        }

        // GET: Cars/Create
        public async Task <IActionResult> Create()
        {
            ViewBag.CarCategoryNameList = new SelectList(await carCategoryRepo.GetAllAsync(), "Id", "Name");
            return View();
        }

        // POST: Cars/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Create ([Bind("CarId,Model,Brand,Color,CarCategoryId")] Car car)
        {
            if (ModelState.IsValid)
            {
               await carRepo.CreateAsync(car);
                return RedirectToAction(nameof(Index));
            }
            return View(car);
        }

        // GET: Cars/Edit/5
        public async Task <IActionResult> Edit(int id)
        {
            if (id == null || carRepo == null)
            {
                return NotFound();
            }

            var car = await carRepo.GetByIdAsync(id);
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
        public async Task <IActionResult> Edit(int id, [Bind("CarId,Model,Brand,Color,CarCategoryId")] Car car)
        {
            if (id != car.CarId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await carRepo.UpdateAsync(car);
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
        public async Task <IActionResult> Delete(int id)
        {
            if (id == null || carRepo == null)
            {
                return NotFound();
            }

            var car = carRepo.GetByIdAsync(id);
                
            if (car == null)
            {
                return NotFound();
            }

            return View(await car);
        }

        // POST: Cars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> DeleteConfirmed(int id)
        {
            if (carRepo == null)
            {
                return Problem("Entity set 'CarRentalContext.Cars'  is null.");
            }
            var car = carRepo.GetByIdAsync(id);
            if (car != null)
            {
                await carRepo.DeleteAsync(car);
            }
            
            return RedirectToAction(nameof(Index));
        }
    }
}
