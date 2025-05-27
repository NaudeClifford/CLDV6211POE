using System.ComponentModel.DataAnnotations;

namespace CLDV6211POEProject.Models
{
    public class EventType
    {
        [Key]
        public int EventTypeID { get; set; }
        public string Name { get; set; }

    }
}
