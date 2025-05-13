using CLDV6211POEProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CLDV6211POEProject.Controllers
{
    public class Bookings1Controller : Controller
    {
        private readonly ApplicationDbContext _context;

        public Bookings1Controller(ApplicationDbContext context)
        {
            _context = context;

        }

        public async Task<IActionResult> Index(string searchString)
        {
            var booking = _context.Bookings1
                .Include(b => b.Event)
                .Include(b => b.Venue)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString)) 
            {
                booking = booking.Where(i =>
                    i.Venue.Venue_Name.Contains(searchString) ||
                    i.Event.Event_Name.Contains(searchString)
                    );
            }

            return View(await booking.ToListAsync());
        }

        public IActionResult Create()
        {
            ViewData["Venue"] = _context.Venue1.ToList();
            ViewData["Event"] = _context.Event1.ToList();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Bookings1 booking)
        {
            var selectedEvent = await _context.Event1.FirstOrDefaultAsync(e => e.Event_Id == @booking.Event_Id);

            if (selectedEvent == null)
            {

                ModelState.AddModelError("", "Selected event not found");
                ViewData["Event"] = _context.Event1.ToList();
                ViewData["Venue"] = _context.Venue1.ToList();
                return View(booking);
            }

            var conflict = await _context.Bookings1
                .AnyAsync(b => b.Venue_Id == @booking.Venue_Id 
                && b.Event.Event_Date == selectedEvent.Event_Date);
               
            if (conflict) 
            {

                ModelState.AddModelError("", "This Venue is already booked for that date.");
                ViewData["Event"] = _context.Event1.ToList();
                ViewData["Venue"] = _context.Venue1.ToList();
                return View(booking);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(booking);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Booking created successfully.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex) 
                {
                    ModelState.AddModelError("", "This venue is already booked for that date.");
                    ViewData["Event"] = _context.Event1.ToList();
                    ViewData["Venue"] = _context.Venue1.ToList();
                    return View(booking);
                }
            }

            ViewData["Event"] = _context.Event1.ToList();
            ViewData["Venue"] = _context.Venue1.ToList();
            return View(booking);
        }

        public async Task<IActionResult> Details(int id)
        {
            

           
            var booking = await _context.Bookings1
                
                .FirstOrDefaultAsync(m => m.Booking_Id == id);

            if (booking == null) return NotFound();
            
            return View(booking);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            var booking = await _context.Bookings1.FirstOrDefaultAsync(m => m.Booking_Id == id);

            if (booking == null) return NotFound();
            
            return View(booking);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var booking = await _context.Bookings1.FindAsync(id);
            _context.Bookings1.Remove(booking);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool EventExist(int id)
        {

            return _context.Bookings1.Any(e => e.Booking_Id == id);
        }

        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null) return NotFound();
            
            var booking = await _context.Bookings1.FindAsync(id);
            
            return View(booking);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Bookings1 booking)
        {

            if (id != booking.Booking_Id) return NotFound();
            

            if (ModelState.IsValid)
            {
                try
                {

                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {

                    if (!EventExist(booking.Booking_Id))
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
    }
}
