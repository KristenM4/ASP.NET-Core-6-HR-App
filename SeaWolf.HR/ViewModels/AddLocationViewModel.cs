using System.ComponentModel.DataAnnotations;

namespace SeaWolf.HR.ViewModels
{
    public class AddLocationViewModel
    {

        [Required(ErrorMessage = "Location name is required")]
        public string LocationName { get; set; }

        [Required(ErrorMessage = "Location phone number is required")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Street address is required")]
        public string AddressLine1 { get; set; }

        public string? AddressLine2 { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }

        [Required(ErrorMessage = "State is required")]
        public string State { get; set; }

        [Required(ErrorMessage = "Postal code(zipcode) is required")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "Country is required")]
        public string Country { get; set; }
    }
}
