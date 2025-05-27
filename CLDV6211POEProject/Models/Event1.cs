using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Mvc;

namespace CLDV6211POEProject.Models
{
    public class Event1
    {
        [Key]
        public int EventID { get; set; }

        [StringLength(100)]
        public required string Event_Name { get; set; }

        [StringLength(100)]
        public required string Description1 { get; set; }
        
        [ForeignKey("VenueID")]
        public int VenueID { get; set; }

        public Venue1? Venue { get; set; }

        [ForeignKey("EventTypeID")]

        public int? EventTypeID { get; set; }

        public EventType? EventType { get; set; }

        public required DateTime Event_Date { get; set; }

    }
}
