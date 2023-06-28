using Microsoft.EntityFrameworkCore;

namespace SeaWolf.HR.Models
{
    public class SeaWolfHRDbContext : DbContext
    {
        public SeaWolfHRDbContext(DbContextOptions<SeaWolfHRDbContext> 
            options) : base(options)
        {   
        }

        public DbSet<Employee> Employees { get; set; }
    }
}
