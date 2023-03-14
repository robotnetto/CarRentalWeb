using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Biluthyrning.Data;
using Biluthyrning.Models;
using Biluthyrning.ViewModels;

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
        public async Task<IActionResult> Index(string search, DateTime? startDateSearch, DateTime? endDateSearch)
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
                bvm.TotalCost = Math.Round(Convert.ToDecimal(span.TotalDays) * bvm.Price, 2);

                if (string.IsNullOrWhiteSpace(search) || bvm.UserName.Contains(search))
                {
                    if(startDateSearch == null && endDateSearch == null || startDateSearch <= bvm.StartDate && endDateSearch > bvm.StartDate
                        || startDateSearch <= bvm.StartDate && endDateSearch == null)
                    {
                        bookingVMList.Add(bvm);
                    }
                        
                    else if (startDateSearch == null && endDateSearch > bvm.StartDate)
                    {
                        startDateSearch = DateTime.Today;
                        if(startDateSearch <= bvm.StartDate && endDateSearch > bvm.StartDate)
                        {
                            bookingVMList.Add(bvm);
                        }
                    }
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
            bvm.UserId = user.UserId;

            if (bvm == null)
            {
                return NotFound();
            }

            return View(bvm);
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookingViewModel myBooking)
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
            ViewBag.UserNameList = new SelectList(await userRep.GetAllAsync(), "UserId", "UserName");
            ViewBag.CarCategory = new SelectList(await carCategoryRep.GetAllAsync(), "Id", "Name");
            var booking = await bookingRep.GetByIdAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            var myBooking = new BookingViewModel();
            myBooking.Id = booking.Id;
            myBooking.CarId = booking.CarId;
            myBooking.StartDate = booking.StartDate;
            myBooking.EndDate = booking.EndDate;
            myBooking.UserId = booking.UserId;

            return View(myBooking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveEdit(int id, [Bind("Id,CarId,StartDate,EndDate,UserId")] BookingViewModel myBooking)
        {
            var booking = await bookingRep.GetByIdAsync(myBooking.Id);

            if (ModelState.IsValid)
            {
                try
                {
                    booking.StartDate = myBooking.StartDate;
                    booking.EndDate = myBooking.EndDate;
                    booking.CarId = myBooking.CarId;
                    booking.UserId = myBooking.UserId;
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
            var myBooking = new BookingViewModel();
            ViewBag.DateValidation = dateValidation;
            ViewBag.UserNameList = new SelectList(await userRep.GetAllAsync(), "UserId", "UserName");
            ViewBag.CarCategory = new SelectList(await carCategoryRep.GetAllAsync(), "Id", "Name");
            ViewBag.UserType = Request.Cookies["UserType"];
            ViewBag.CurrentUserId = Request.Cookies["CurrentUserId"];
            ViewBag.CarCategory = new SelectList(await carCategoryRep.GetAllAsync(), "Id", "Name");

            return View(myBooking);
        }
    
        public async Task<IActionResult> SelectCar(BookingViewModel myBooking)
        {
            bool validDate = IsValidDate(myBooking.StartDate, myBooking.EndDate);
            if (!validDate)
            {
                return RedirectToAction("SetDates", new { dateValidation = false });
            }
            await AvailableCars(myBooking);
            if (myBooking.Cars.Any(x => x.IsAvailable == true))
            {
                var user = await userRep.GetByIdAsync(myBooking.UserId);
                myBooking.UserName = user.UserName;
                ViewBag.AvailableCars = new SelectList(myBooking.Cars, "CarId", "Model");
                return View(myBooking);
            }
            else
            {
                ViewBag.DateValidation = true;
                ViewBag.UserType = Request.Cookies["UserType"];
                ViewBag.CurrentUserId = Request.Cookies["CurrentUserId"];
                ViewBag.UserNameList = new SelectList(await userRep.GetAllAsync(), "UserId", "UserName");
                ViewBag.CarCategory = new SelectList(await carCategoryRep.GetAllAsync(), "Id", "Name");
                ModelState.AddModelError("", "No cars available during that time.");
                return View(nameof(SetDates), myBooking);
            }
        }

        public async Task<IActionResult> ConfirmBooking(BookingViewModel myBooking, string submit)
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
            myBooking.TotalCost = Math.Round(Convert.ToDecimal(span.TotalDays) * myBooking.Price, 2);
            return View(myBooking);
        }

        private async Task<List<Car>> AvailableCars(BookingViewModel myBooking)
        {
            var bookings = await bookingRep.GetAllAsync();
            var cars = await carRep.GetAllAsync();
            foreach (var car in cars.ToList())
            {
                if (myBooking.CarCategoryId == car.CarCategoryId)
                {
                    car.IsAvailable = true;
                }
            }
            if (myBooking.Id == 0)
            {

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
            }
            else
            {
                foreach (var booking in bookings.Where(s => s.Id != myBooking.Id))
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
        private bool IsValidDate(DateTime? startDate, DateTime? endDate)
        {
            return startDate < endDate && endDate > startDate;
        }
        public async Task<IActionResult> ConfirmEdit(BookingViewModel myBooking, string submit)
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
            myBooking.TotalCost = Math.Round(Convert.ToDecimal(span.TotalDays) * myBooking.Price, 2);
            return View(myBooking);
        }
        public async Task<IActionResult> EditCar(int id, BookingViewModel myBooking)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.UserType = Request.Cookies["UserType"];
                ViewBag.UserNameList = new SelectList(await userRep.GetAllAsync(), "UserId", "UserName");
                return View(nameof(Edit), myBooking);
            }
           
            var valid = IsValidDate(myBooking.StartDate, myBooking.EndDate);
            if (!valid)
            {
                ViewBag.UserType = Request.Cookies["UserType"];
                ViewBag.UserNameList = new SelectList(await userRep.GetAllAsync(), "UserId", "UserName");
                ViewBag.CarCategory = new SelectList(await carCategoryRep.GetAllAsync(), "Id", "Name");
                ModelState.AddModelError("", "Startdate must be set to before enddate");
                return View(nameof(Edit), myBooking);
            }
            
            await AvailableCars(myBooking);
            return View(myBooking);
        }

        public async Task<IActionResult> Info()
        {
            return View();
        }
    }
}
