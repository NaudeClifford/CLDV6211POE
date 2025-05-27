using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security;
using Microsoft.EntityFrameworkCore;

namespace CLDV6211POEProject.Models
{
    public class Bookings1
    {
        [Key]
        public int BookingID { get; set; }

        public int EventID { get; set; } 

        [ForeignKey("EventID")]
        public Event1? Event { get; set; }

        public int VenueID { get; set; }   
        
        [ForeignKey("VenueID")]
        public Venue1? Venue { get; set; }

        public required DateTime Booking_Date { get; set; }

    }
}
