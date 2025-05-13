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

        public async Task<IActionResult> Index()
        {
            var event1 = await _context.Event1.ToListAsync();

            return View(event1);
        }

        public IActionResult Create() 
        {
            ViewData["Venue"] = _context.Venue1.ToList();
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

            return View(@event2);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var @event2 = await _context.Event1
                .FirstOrDefaultAsync(m => m.Event_Id == id); 

            if (@event2 == null) return NotFound();
            

            return View(@event2);
        }

        public async Task<IActionResult> Delete(int? id)
        {

            if (id == null) return NotFound();

            var @event2 = await _context.Event1
                .FirstOrDefaultAsync(m => m.Event_Id == id);

            if (@event2 == null) return NotFound();
            
            return View(@event2);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfimed(int id)
        {
            var @event = await _context.Event1.FindAsync(id);

            if (@event == null) return NotFound();

            var isBooked = await _context.Bookings1.AnyAsync(b => b.Event_Id == id);

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
        
        return _context.Event1.Any(e => e.Event_Id == id);
        }

        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null) return NotFound();
            
            var event2 = await _context.Event1.FindAsync(id);

            if (id == null) return NotFound();

            ViewData["Venue"] = _context.Venue1.ToList();

            return View(event2);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Event1 event2)
        {

            if (id != event2.Event_Id) return NotFound();
            
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

                    if (!EventExist(event2.Event_Id))
                    {

                        return NotFound();
                    }
                    else 
                    {

                        throw;
                    }
                }
            }
            return View(event2);
        }



    }
}
