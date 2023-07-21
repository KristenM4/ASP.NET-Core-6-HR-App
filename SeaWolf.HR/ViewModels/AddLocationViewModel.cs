using SeaWolf.HR.Models;
using System.ComponentModel.DataAnnotations;

namespace SeaWolf.HR.ViewModels
{
    public class AddLocationViewModel
    {

        [Required]
        public string LocationName { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string AddressLine1 { get; set; }

        public string? AddressLine2 { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string PostalCode { get; set; }

        [Required]
        public string Country { get; set; }
    }
}
