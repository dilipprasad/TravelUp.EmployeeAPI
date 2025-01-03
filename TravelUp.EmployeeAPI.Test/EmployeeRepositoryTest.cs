using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using TravelUp.EmployeeAPI.Data;
using TravelUp.EmployeeAPI.Data.Models;
using TravelUp.EmployeeAPI.Data.Repository;

namespace TravelUp.EmployeeAPI.Test
{
    [TestClass]
    public class EmployeeRepositoryTests
    {
        private ApplicationDbContext _context;
        private IEmployeeRepository _repository;
        private ILogger<EmployeeRepository> _logger;
        private IConfiguration _config;


        [TestInitialize]
        public void Setup()
        {
            _config = MockRepository.GenerateMock<IConfiguration>();

            // Use an in-memory database for testing
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new ApplicationDbContext(options, _config);

            // Mock logger using Rhino Mocks
            _logger = MockRepository.GenerateMock<ILogger<EmployeeRepository>>();

            // Initialize repository
            _repository = new EmployeeRepository(_context, _logger);
        }


        [TestMethod]
        public async Task GetAllAsync_ShouldReturnAllEmployees()
        {
            // Arrange
            _context.Employees.AddRange(new List<Employee>
                {
                    new Employee { Id = Guid.NewGuid(), Name = "John Doe", Email = "john.doe@example.com" },
                    new Employee { Id = Guid.NewGuid(), Name = "Jane Doe", Email = "jane.doe@example.com" }
                });
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public async Task GetByIdAsync_ShouldReturnEmployee_ForValidId()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            _context.Employees.Add(new Employee { Id = employeeId, Name = "John Doe", Email = "john.doe@example.com" });
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByIdAsync(employeeId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("John Doe", result.Name);
        }


        [TestMethod]
        public async Task CreateAsync_ShouldAddNewEmployee()
        {
            // Arrange
            var newEmployee = new Employee { Name = "John Doe", Email = "john.doe@example.com" };

            // Act
            var result = await _repository.CreateAsync(newEmployee);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreNotEqual(Guid.Empty, result.Id);

            var createdEmployee = await _context.Employees.FindAsync(result.Id);
            Assert.IsNotNull(createdEmployee);
            Assert.AreEqual("John Doe", createdEmployee.Name);
        }

        [TestMethod]
        public async Task UpdateAsync_ShouldUpdateExistingEmployee()
        {
            // Arrange
            var employee = new Employee { Id = Guid.NewGuid(), Name = "John Doe", Email = "john.doe@example.com" };
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            // Act
            employee.Name = "John Updated";
            var result = await _repository.UpdateAsync(employee);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("John Updated", result.Name);

            var updatedEmployee = await _context.Employees.FindAsync(employee.Id);
            Assert.AreEqual("John Updated", updatedEmployee.Name);
        }

        [TestMethod]
        public async Task DeleteAsync_ShouldDeleteEmployee_ForValidId()
        {
            // Arrange
            var employee = new Employee { Id = Guid.NewGuid(), Name = "John Doe" };
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.DeleteAsync(employee.Id);

            // Assert
            Assert.IsTrue(result);
            Assert.IsNull(await _context.Employees.FindAsync(employee.Id));
        }

      


    }

}