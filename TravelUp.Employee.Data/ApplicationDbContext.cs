using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TravelUp.EmployeeAPI.Data.Models;

namespace TravelUp.EmployeeAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        private readonly DbContextOptions<ApplicationDbContext> _options;
        private readonly IConfiguration _config;

        // The constructor that receives DbContextOptions
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration config)
            : base(options)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _options = options ?? throw new ArgumentNullException(nameof(options));
           
        }

        // Employees table
        public DbSet<Employee> Employees { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {


            if (!optionsBuilder.IsConfigured)
            {
                // Read the connection string from configuration
                var connectionString = _config.GetConnectionString("DefaultConnection");
                optionsBuilder.UseSqlServer(connectionString);
            }

        }
    }
}
