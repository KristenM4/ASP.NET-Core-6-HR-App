using SeaWolf.HR.Models;
using System.ComponentModel.DataAnnotations;

namespace SeaWolf.HR.ViewModels
{
    public class AddEmployeeViewModel
    {
        public int EmployeeId { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string? MiddleName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Position { get; set; }

        [Required]
        public string Location { get; set; }
    }
}
