using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CLDV6211POEProject.Models
{
    public class Venue1
    {
        [Key]
        public int VenueID { get; set; }
        
        [StringLength(100)]
        public required string Venue_Name { get; set; }
        
        [StringLength(100)]
        public required string Location1 { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Capacity must be greater than 0")]
        public required int Capacity { get; set; }

        [StringLength(100)]
        public string? ImageURL { get; set; }

        [NotMapped]

        public IFormFile? ImageFile { get; set; }


    }
}
