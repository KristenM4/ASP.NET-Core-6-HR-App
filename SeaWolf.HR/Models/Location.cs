using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SeaWolf.HR.Models
{
    public class Location
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public string Phone { get; set; }
        public List<Employee>? Employees { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set;} = string.Empty;
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
    }
}
