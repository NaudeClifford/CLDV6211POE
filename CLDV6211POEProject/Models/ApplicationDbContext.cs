using Microsoft.EntityFrameworkCore;

namespace CLDV6211POEProject.Models
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Event1> Event1 { get; set; }
        public DbSet<Venue1> Venue1 { get; set; }
        public DbSet<Bookings1> Bookings1 { get; set; }
        public DbSet<EventType> EventType { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            :base(options) 
        {
        
        
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EventType>().HasData(
                new EventType { EventTypeID = 1, Name = "Conference" },
                new EventType { EventTypeID = 2, Name = "Wedding" },
                new EventType { EventTypeID = 3, Name = "Dance" }

                );
        }

    }
}
