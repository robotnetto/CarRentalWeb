using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Biluthyrning.Data;
using Biluthyrning.Models;
using Biluthyrning.ViewModels;
using Azure.Identity;

namespace Biluthyrning.Controllers
{
    public class BookingsController : Controller
    {
        private readonly IBooking bookingRep;
        private readonly ICar carRep;
        private readonly IUser userRep;
        private readonly ICarCategory carCategoryRep;

        public BookingsController(IBooking bookingRep, ICar carRep, IUser userRep, ICarCategory carCategoryRep)
        {
            this.bookingRep = bookingRep;
            this.carRep = carRep;
            this.userRep = userRep;
            this.carCategoryRep = carCategoryRep;
        }

        // GET: Bookings
        public async Task<IActionResult> Index()
        {
            ViewBag.UserType = Request.Cookies["UserType"];
            ViewData["CurrentUserId"] = Request.Cookies["CurrentUserId"];
            var bookingVMList = new List<BookingViewModel>();
            foreach (var item in await bookingRep.GetAllAsync())
            {
                var bvm = new BookingViewModel();
                var car = await carRep.GetByIdAsync(item.CarId);
                var user = await userRep.GetByIdAsync(item.UserId);
                var carCategory = await carCategoryRep.GetByIdAsync(car.CarCategoryId);
                bvm.Id = item.Id;
                bvm.CarId = item.CarId;
                bvm.CarModel = car.Model;
                bvm.CarBrand = car.Brand;
                bvm.StartDate = item.StartDate;
                bvm.EndDate = item.EndDate;
                bvm.UserName = user.UserName;
                bvm.UserId= item.UserId;
                bvm.CarCategoryName = carCategory.Name;
                bookingVMList.Add(bvm);
            }
            return View(bookingVMList);
        }
        
        // GET: Bookings/Details/5
        public async Task<IActionResult> Details(int id)
        {
            ViewBag.UserType = Request.Cookies["UserType"];
            if (id == null)
            {
                return NotFound();
            }
            
            var bvm = new BookingViewModel();
            var booking = await bookingRep.GetByIdAsync(id);
            var car = await carRep.GetByIdAsync(booking.CarId);
            var user = await userRep.GetByIdAsync(booking.UserId);
            var carCategory = await carCategoryRep.GetByIdAsync(car.CarCategoryId);
            bvm.Id = id;
            bvm.CarId = car.CarId;
            bvm.CarModel = car.Model;
            bvm.CarBrand = car.Brand;
            bvm.StartDate = booking.StartDate;
            bvm.EndDate = booking.EndDate;
            bvm.UserName = user.UserName;
            bvm.CarCategoryName = carCategory.Name;

            if (bvm == null)
            {
                return NotFound();
            }

            return View(bvm);
        }

        // GET: Bookings/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.UserType = Request.Cookies["UserType"];
            ViewBag.CurrentUserId = Request.Cookies["CurrentUserId"];

            ViewBag.UserNameList = new SelectList(await userRep.GetAllAsync(), "UserId", "UserName");
            //TODO: Vill vi se ngt annat än bilarnas Id i Booking/Create?
            ViewBag.CarIdList = new SelectList(await carRep.GetAllAsync(), "CarId", "CarId");
            return View();
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CarId,StartDate,EndDate,UserId")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                await bookingRep.AddAsync(booking);
                return RedirectToAction(nameof(Index));
            }
            return View(booking);
        }

        // GET: Bookings/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            ViewBag.UserType = Request.Cookies["UserType"];

            if (id == null)
            {
                return NotFound();
            }

            var booking = await bookingRep.GetByIdAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CarId,StartDate,EndDate,UserId")] Booking booking)
        {
            if (id != booking.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await bookingRep.UpdateAsync(booking);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(booking);
        }

        // GET: Bookings/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            ViewBag.UserType = Request.Cookies["UserType"];
            //TODO: vill vi visa carmodel, carBrand och UserName på Delete sidan också?
            if (id == null)
            {
                return NotFound();
            }

            var booking = await bookingRep.GetByIdAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await bookingRep.GetByIdAsync(id);
            if (booking != null)
            {
                await bookingRep.DeleteAsync(id);
            }
            else
            {
                return Problem("Booking does not exist.");
            }
            
            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
            var booking = bookingRep.GetByIdAsync(id);

            if(booking != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
