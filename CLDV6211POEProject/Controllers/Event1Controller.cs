using CLDV6211POEProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace CLDV6211POEProject.Controllers
{
    public class Event1Controller : Controller
    {

        private readonly ApplicationDbContext _context;

        public Event1Controller(ApplicationDbContext context) 
        {
            _context = context;
        
        }

        public async Task<IActionResult> Index(string searchType, int? Venue_Id, DateTime? startDate, DateTime? endDate)
        {
            var event1 = _context.Event1
                .Include(e => e.Venue)
                .Include(e => e.EventType)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchType))
                event1 = event1.Where(event1 => event1.EventType.Name == searchType);

            if (Venue_Id.HasValue)
                event1 = event1.Where(event1 => event1.VenueID == Venue_Id);

            if(startDate.HasValue && endDate.HasValue)
                event1 = event1.Where(event1 => event1.Event_Date >= startDate && event1.Event_Date <= endDate);

            ViewData["EventType"] = _context.EventType.ToList();
            ViewData["Venue"] = _context.Venue1.ToList();

            return View(await event1.ToListAsync());
        }

        public IActionResult Create() 
        {
            ViewData["Venue"] = _context.Venue1.ToList();
            ViewData["EventType"] = _context.EventType.ToList();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Event1 @event2)
        {

            if (ModelState.IsValid)
            {

                _context.Add(@event2);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Event created successfully";
                return RedirectToAction(nameof(Index));
            }

            ViewData["Venue"] = _context.Venue1.ToList();

            ViewData["EventType"] = _context.EventType.ToList();

            return View(@event2);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var @event2 = await _context.Event1
                .Include(e => e.Venue)
                .FirstOrDefaultAsync(m => m.EventID == id); 

            if (@event2 == null) return NotFound();
            

            return View(@event2);
        }

        public async Task<IActionResult> Delete(int? id)
        {

            if (id == null) return NotFound();

            var @event2 = await _context.Event1
                .Include(e => e.Venue)
                .FirstOrDefaultAsync(m => m.EventID == id);

            if (@event2 == null) return NotFound();
            
            return View(@event2);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfimed(int id)
        {
            var @event = await _context.Event1.FindAsync(id);

            if (@event == null) return NotFound();

            var isBooked = await _context.Bookings1.AnyAsync(b => b.EventID == id);

            if (isBooked)
            {
                TempData["ErrorMessage"] = "Cannot delete Event because it has existing bookings";
                return RedirectToAction(nameof(Index));
            }

            _context.Event1.Remove(@event);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Event delected successfully";
            return RedirectToAction(nameof(Index));

        }

        private bool EventExist(int id)
        { 
        
        return _context.Event1.Any(e => e.EventID == id);
        }

        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null) return NotFound();
            
            var event2 = await _context.Event1.FindAsync(id);

            if (id == null) return NotFound();

            ViewData["Venue"] = _context.Venue1.ToList();

            ViewData["EventType"] = _context.EventType.ToList();

            return View(event2);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Event1 event2)
        {

            if (id != event2.EventID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {

                    _context.Update(event2);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Event updated successfully";
                    return RedirectToAction(nameof(Index));

                }
                catch (DbUpdateConcurrencyException)
                {

                    if (!EventExist(event2.EventID))
                    {

                        return NotFound();
                    }
                    else
                    {

                        throw;
                    }
                }

            }

            ViewData["Venue"] = _context.Venue1.ToList();

            ViewData["EventType"] = _context.EventType.ToList();
            
            return View(event2);
        }



    }
}
