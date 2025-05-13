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
        public int Event_Id { get; set; }

        [StringLength(100)]
        public required string Event_Name { get; set; }

        [StringLength(100)]
        public required string Description1 { get; set; }
        
        [ForeignKey("Venue_Id")]
        public int Venue_Id { get; set; }

        public required DateTime Event_Date { get; set; }

        
    }
}
