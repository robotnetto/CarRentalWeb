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
using Microsoft.CodeAnalysis.Elfie.Extensions;

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
        public async Task<IActionResult> Index(string search)
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
                bvm.UserId = item.UserId;
                bvm.CarCategoryName = carCategory.Name;
                bvm.Price = car.Amount;
                TimeSpan span = bvm.EndDate - bvm.StartDate;
                bvm.TotalCost = Convert.ToDecimal(span.TotalDays) * bvm.Price;

                if (string.IsNullOrWhiteSpace(search) || bvm.UserName.Contains(search))
                {
                    bookingVMList.Add(bvm);
                }


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
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,CarId,StartDate,EndDate,UserId")] Booking booking)
        //{
        //    bool validDate = IsValidDate(booking.StartDate, booking.EndDate);
        //    if (ModelState.IsValid && validDate)
        //    {
        //        await bookingRep.AddAsync(booking);
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return RedirectToAction(nameof(Create));
        //}


        // POST: Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ConfirmBookingVM myBooking)
        {
            var booking = new Booking();
            booking.StartDate = myBooking.StartDate;
            booking.EndDate = myBooking.EndDate;
            booking.CarId = myBooking.CarId;
            booking.UserId = myBooking.UserId;
            await bookingRep.AddAsync(booking);
            return RedirectToAction(nameof(Index));
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



        public async Task<IActionResult> SetDates(bool dateValidation = true)
        {
            var myBooking = new ConfirmBookingVM();
            ViewBag.DateValidation = dateValidation;
            ViewBag.UserNameList = new SelectList(await userRep.GetAllAsync(), "UserId", "UserName");
            ViewBag.UserType = Request.Cookies["UserType"];
            ViewBag.CurrentUserId = Request.Cookies["CurrentUserId"];
            return View(myBooking);
        }
        public async Task<IActionResult> SelectCar(ConfirmBookingVM myBooking)
        {
            bool validDate = IsValidDate(myBooking.StartDate, myBooking.EndDate);
            if (!validDate)
            {
                return RedirectToAction("SetDates", new { dateValidation = false });
            }
            await AvailableCars(myBooking);
            var user = await userRep.GetByIdAsync(myBooking.UserId);
            myBooking.UserName = user.UserName;
            ViewBag.AvailableCars = new SelectList(myBooking.Cars, "CarId", "Model");
            return View(myBooking);
        }

        public async Task<IActionResult> ConfirmBooking(ConfirmBookingVM myBooking, string submit)
        {
            ViewBag.UserType = Request.Cookies["UserType"];

            myBooking.CarId = Convert.ToInt32(submit);
            var user = await userRep.GetByIdAsync(myBooking.UserId);
            var car = await carRep.GetByIdAsync(myBooking.CarId);

            myBooking.UserName = user.UserName;
            myBooking.CarBrand = car.Brand;
            myBooking.CarModel = car.Model;
            myBooking.Price = car.Amount;
            TimeSpan span = myBooking.EndDate - myBooking.StartDate;
            myBooking.TotalCost = Convert.ToDecimal(span.TotalDays) * myBooking.Price;
            return View(myBooking);
        }

        private async Task<List<Car>> AvailableCars(ConfirmBookingVM myBooking)
        {
            var bookings = await bookingRep.GetAllAsync();
            var cars = await carRep.GetAllAsync();
            foreach (var car in cars.ToList())
            {
                car.IsAvailable = true;
            }
            foreach (var booking in bookings)
            {
                if (myBooking.StartDate <= booking.EndDate && myBooking.StartDate >= booking.StartDate
                    || myBooking.EndDate >= booking.StartDate && myBooking.EndDate <= booking.EndDate
                    || myBooking.StartDate <= booking.StartDate && myBooking.EndDate >= booking.EndDate)
                {
                    foreach (var car in cars)
                    {
                        if (car.CarId == booking.CarId)
                        {
                            car.IsAvailable = false;
                        }
                    }
                }
            }
            myBooking.Cars = cars.ToList();
            return myBooking.Cars;
        }

        private bool BookingExists(int id)
        {
            var booking = bookingRep.GetByIdAsync(id);

            if (booking != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool IsValidDate(DateTime startDate, DateTime endDate)
        {
            return startDate < endDate && endDate > startDate;
        }
    }
}
