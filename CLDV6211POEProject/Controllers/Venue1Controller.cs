using System.Net.Mime;
using CLDV6211POEProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CLDV6211POEProject.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Azure.Storage.Blobs;

namespace CLDV6211POEProject.Controllers
{
    public class Venue1Controller : Controller
    {
        private readonly ApplicationDbContext _context;

        public Venue1Controller(ApplicationDbContext context)
        {
            _context = context;

        }

        public async Task<IActionResult> Index()
        {
            var venue = await _context.Venue1.ToListAsync();
            return View(venue);
        }

        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Venue1 venue)
        {

            if (ModelState.IsValid)
            {

                if (venue.ImageFile != null) {

                    var blobUrl = await UploadImageToBlobAsync(venue.ImageFile);

                    venue.ImageURL = blobUrl;

                }
                _context.Venue1.Add(venue);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Venue created successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View(venue);
        }

        public async Task<IActionResult> Details(int? id)
        {

            if (id == null) return NotFound();
            
            var venue = await _context.Venue1.FirstOrDefaultAsync(m => m.VenueID == id);

            if (venue == null) return NotFound();

            return View(venue);
        }

        public async Task<IActionResult> Delete(int? id)
        {

            if (id == null) return NotFound();
            
            var venue = await _context.Venue1.FirstOrDefaultAsync(m => m.VenueID == id);

            if (venue == null) return NotFound();

            return View(venue);

        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfimed(int id)
        {
            var venue = await _context.Venue1.FindAsync(id);

            if (venue == null) return NotFound();

            var hasBookings = await _context.Bookings1.AnyAsync(b => b.VenueID == id);

            if (hasBookings) 
            {
                TempData["ErrorMessage"] = "Cannot delecte venue because it has existing bookings";
                return RedirectToAction(nameof(Index));
            }

            _context.Venue1.Remove(venue);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Venue delected successfully";
            return RedirectToAction(nameof(Index));

        }

        private bool EventExist(int id)
        {

            return _context.Venue1.Any(e => e.VenueID == id);
        }

        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null) return NotFound();
            

            var venue = await _context.Venue1.FindAsync(id);

            if (id == null) return NotFound();
            
            return View(venue);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Venue1 venue)
        {

            if (id != venue.VenueID) return NotFound();
            
            if (ModelState.IsValid)
            {
                try
                {

                    if (venue.ImageFile != null)
                    {

                        var blobUrl = await UploadImageToBlobAsync(venue.ImageFile);

                        venue.ImageURL = blobUrl;
                    }

                    else 
                    {

                    }

                    _context.Update(venue);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Venue updated successfully";
                }
                catch (DbUpdateConcurrencyException)
                {

                    if (!VenueExist(venue.VenueID)) return NotFound();
                    
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(venue);
        }

       private async Task<string> UploadImageToBlobAsync(IFormFile imageFile)
        {
            var connectionString = "DefaultEndpointsProtocol=https;AccountName=account23blob;AccountKey=DXZe1AhoYuSXUWqKxDAAHmOkJ/MNd96qLdZUaYtNSHGTY/HAJNctpgsvToRBKn6tuYxpyxkIl7Oh+ASty7HMNg==;EndpointSuffix=core.windows.net";
            var containerName = "blog8767";

            var blobServiceClient = new BlobServiceClient(connectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(Guid.NewGuid() + Path.GetExtension(imageFile.FileName));

            var blobHttpHeaders = new Azure.Storage.Blobs.Models.BlobHttpHeaders
            {
                ContentType = imageFile.ContentType
            };

            using (var stream = imageFile.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, new Azure.Storage.Blobs.Models.BlobUploadOptions
                {
                    HttpHeaders = blobHttpHeaders
                });
            }

            return blobClient.Uri.ToString();
        }

        private bool VenueExist(int id) 
        {
            return _context.Venue1.Any(equals => equals.VenueID == id);
        }
    }
}
