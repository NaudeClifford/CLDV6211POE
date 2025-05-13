using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security;
using Microsoft.EntityFrameworkCore;

namespace CLDV6211POEProject.Models
{
    public class Bookings1
    {
        [Key]
        public int Booking_Id { get; set; }

        public int Event_Id { get; set; } 

        [ForeignKey("Event_Id")]
        public Event1 Event { get; set; }

        public int Venue_Id { get; set; }   
        
        [ForeignKey("Venue_Id")]
        public Venue1 Venue { get; set; }

        public required DateTime Booking_Date { get; set; }




    }
}
