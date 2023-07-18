using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SeaWolf.HR.Models
{
    public class SeaWolfHRDbContext : IdentityDbContext<HRUser>
    {
        public SeaWolfHRDbContext(DbContextOptions<SeaWolfHRDbContext> 
            options) : base(options)
        {   
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Location> Locations { get; set; }
    }
}
