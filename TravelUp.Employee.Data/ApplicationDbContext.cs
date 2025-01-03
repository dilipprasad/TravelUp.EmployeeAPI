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
            _options = options;
             _config = config;
        }

        // Employees table
        public DbSet<Employee> Employees { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            //    //optionsBuilder.UseSqlServer("Server=tcp:travelup.database.windows.net,1433;Initial Catalog=EmployeeInfo;Persist Security Info=False;User ID=adminuser;Password=TravelUpUser1122###;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

            if (!optionsBuilder.IsConfigured)
            {
                // Read the connection string from configuration
                var connectionString = _config.GetConnectionString("DefaultConnection");
                optionsBuilder.UseSqlServer(connectionString);
            }

        }
    }
}
