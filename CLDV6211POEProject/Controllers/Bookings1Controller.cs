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
                .Include(c => c.Event)
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
            var selectedEvent = await _context.Event1.FirstOrDefaultAsync(e => e.EventID == booking.EventID);

            if (selectedEvent == null)
            {
                ModelState.AddModelError("", "Selected event not found.");
                ViewData["Event"] = _context.Event1.ToList();
                ViewData["Venue"] = _context.Venue1.ToList();
                return View(booking);
            }

            // Check manually for double booking
            var conflict = await _context.Bookings1
                .Include(b => b.Event)
                .AnyAsync(b => b.VenueID == booking.VenueID &&
                               b.Event.Event_Date.Date == selectedEvent.Event_Date.Date);

            if (conflict)
            {
                ModelState.AddModelError("", "This venue is already booked for that date.");
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
                    // If database constraint fails (e.g., unique key violation), show friendly message
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
                
                .FirstOrDefaultAsync(m => m.BookingID == id);

            if (booking == null) return NotFound();
            
            return View(booking);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            var booking = await _context.Bookings1.FirstOrDefaultAsync(m => m.BookingID == id);

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

            return _context.Bookings1.Any(e => e.BookingID == id);
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

            if (id != booking.BookingID) return NotFound();
            

            if (ModelState.IsValid)
            {
                try
                {

                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {

                    if (!EventExist(booking.BookingID))
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
