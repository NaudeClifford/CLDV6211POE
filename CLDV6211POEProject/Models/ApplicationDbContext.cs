using Microsoft.EntityFrameworkCore;

namespace CLDV6211POEProject.Models
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Event1> Event1 { get; set; }
        public DbSet<Venue1> Venue1 { get; set; }
        public DbSet<Bookings1> Bookings1 { get; set; }

       
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            :base(options) 
        {
        
        
        }

    }
}
