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
    public class BookingsController : Controller
    {
        private readonly IBooking bookingRep;

        public BookingsController(IBooking bookingRep)
        {
            this.bookingRep = bookingRep;
        }

        // GET: Bookings
        public async Task<IActionResult> Index()
        {
            return View(bookingRep.GetAll());
        }

        // GET: Bookings/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = bookingRep.GetById(id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // GET: Bookings/Create
        public IActionResult Create()
        {
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
                bookingRep.Add(booking);
                return RedirectToAction(nameof(Index));
            }
            return View(booking);
        }

        // GET: Bookings/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = bookingRep.GetById(id);
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
                    bookingRep.Update(booking);
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
            if (id == null)
            {
                return NotFound();
            }

            var booking = bookingRep.GetById(id);
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
            var booking = bookingRep.GetById(id);
            if (booking != null)
            {
                bookingRep.Delete(id);
            }
            else
            {
                return Problem("Booking does not exist.");
            }
            
            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
            var booking = bookingRep.GetById(id);

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
