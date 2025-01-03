
using Microsoft.EntityFrameworkCore;
using TravelUp.EmployeeAPI.Data;
using TravelUp.EmployeeAPI.Data.Repository;

namespace TravelUp.EmployeeAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            IConfigurationRoot? config;
#if DEBUG
            config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
#else
            config = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile($"appsettings.{environmentName}.json")
                            .Build();
#endif

          

            // 3. Register the repository
            builder.Services.AddSingleton<IConfiguration>(config);
            builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

            // 2. Add DbContext to the services container.
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
                );

            // 3. Add controllers (including minimal API support if needed).
            builder.Services.AddControllers();


            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
