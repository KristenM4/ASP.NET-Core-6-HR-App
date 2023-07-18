using Microsoft.AspNetCore.Identity;

namespace SeaWolf.HR.Models
{
    public class HRUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
