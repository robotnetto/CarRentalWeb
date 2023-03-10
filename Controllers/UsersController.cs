using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Biluthyrning.Data;
using Biluthyrning.Models;
using Biluthyrning.ViewModels;

namespace Biluthyrning.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUser userRepo;
        private readonly IBooking bookingRepo;
        private readonly ICar carRepo;

        public UsersController(IUser userRepo, IBooking bookingRepo, ICar carRepo)
        {
            this.userRepo = userRepo;
            this.bookingRepo = bookingRepo;
            this.carRepo = carRepo;
        }

        // GET: Users
        public async Task<IActionResult> Index(string search)
        {
            ViewBag.UserType = Request.Cookies["UserType"];
            ViewData["CurrentUserId"] = Request.Cookies["CurrentUserId"];
            ViewBag.SearchUser = search;
            var users = await userRepo.GetAllAsync();
            var userVM = new UserVM();
            
            if (string.IsNullOrWhiteSpace(search))
            {
                return View(await userRepo.GetAllAsync());
            }
            else
            {                
                return View(await userRepo.GetSearchedAsync(search));
            }
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewBag.UserType = Request.Cookies["UserType"];
            if (id == null || userRepo == null)
            {
                return NotFound();
            }

            var user = await userRepo.GetByIdAsync(id);
            var userVM = new UserVM();

            userVM.UserId = user.UserId;
            userVM.UserName = user.UserName;
            userVM.Password = user.Password;
            userVM.IsAdmin = user.IsAdmin;
            userVM.Email = user.Email;
            userVM.PhoneNr = user.PhoneNr;

            foreach (var booking in await bookingRepo.GetAllAsync())
            {
                if (booking.UserId == userVM.UserId)
                {
                    var bookingVM = new BookingViewModel();
                    bookingVM.StartDate = booking.StartDate;
                    bookingVM.EndDate = booking.EndDate;
                    bookingVM.CarId = booking.CarId;
                    bookingVM.Id = booking.Id;
                    foreach (var car in await carRepo.GetAllAsync())
                    {
                        if (car.CarId == bookingVM.CarId)
                        {
                            bookingVM.CarBrand = car.Brand;
                            bookingVM.CarModel = car.Model;
                        }
                    }
                    userVM.Bookings.Add(bookingVM);
                }
            }
            if (user == null)
            {
                return NotFound();
            }
            return View(userVM);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            ViewBag.UserType = Request.Cookies["UserType"];
            ViewData["CurrentUserId"] = Request.Cookies["CurrentUserId"];
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,UserName,Password,IsAdmin,Email, PhoneNr")] User user)
        {
            if (ModelState.IsValid)
            {
                await userRepo.AddAsync(user);
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.UserType = Request.Cookies["UserType"];
            if (id == null || userRepo == null)
            {
                return NotFound();
            }

            var user = await userRepo.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,UserName,Password,IsAdmin,Email,PhoneNr")] User user)
        {
            if (id != user.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await userRepo.UpdateAsync(user);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await UserExists(user.UserId))
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
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            ViewBag.UserType = Request.Cookies["UserType"];
            if (id == null || userRepo == null)
            {
                return NotFound();
            }

            var user = await userRepo.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (userRepo == null)
            {
                return Problem("Entity set 'CarRentalContext.Users'  is null.");
            }
            var user = await userRepo.GetByIdAsync(id);
            if (user != null)
            {
                await userRepo.DeleteAsync(id);
            }
            return RedirectToAction(nameof(Index));
        }
        private async Task<bool> UserExists(int id)
        {
            var tempUser = await userRepo.GetByIdAsync(id);
            if (tempUser == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
