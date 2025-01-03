using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using TravelUp.EmployeeAPI.Data.Repository;
using TravelUp.EmployeeAPI.Data.Models;
using TravelUp.EmployeeAPI.Controllers;

namespace TravelUp.EmployeeAPI.Test
{
    [TestClass]
    public class EmployeeControllerTests
    {
        private IEmployeeRepository _mockRepo;
        private ILogger<EmployeeController> _mockLogger;
        private EmployeeController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockRepo = MockRepository.GenerateMock<IEmployeeRepository>();
            _mockLogger = MockRepository.GenerateMock<ILogger<EmployeeController>>();
            _controller = new EmployeeController(_mockRepo, _mockLogger);
        }

        [TestMethod]
        public async Task GetAllEmployees_ReturnsOkResult_WithEmployees()
        {
            // Arrange
            var employees = new List<Employee>
            {
                new Employee { Id = Guid.NewGuid(), Name = "John Doe", Email = "john.doe@example.com" },
                new Employee { Id = Guid.NewGuid(), Name = "Jane Smith", Email = "jane.smith@example.com" }
            };
            _mockRepo.Stub(repo => repo.GetAllAsync()).Return(Task.FromResult((IEnumerable<Employee>)employees));

            // Act
            var result = await _controller.GetAllEmployees();

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnValue = okResult.Value as List<Employee>;
            Assert.AreEqual(2, returnValue.Count);
        }

        [TestMethod]
        public async Task GetPaginated_ReturnsOkResult_WithPaginatedData()
        {
            var data  = (new List<Employee> { new Employee { Id = Guid.NewGuid(), Name = "John Doe" } }) ;
            // Arrange
            var paginatedResult = new PaginatedResult<Employee>(data, 10, 1, 1);
               
            
            _mockRepo.Stub(repo => repo.GetPaginatedAsync(1, 10)).Return(Task.FromResult(paginatedResult));

            // Act
            var result = await _controller.GetPaginated(1, 10);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnValue = okResult.Value as PaginatedResult<Employee>;
            Assert.AreEqual(1, returnValue.Items.Count());
        }

        [TestMethod]
        public async Task GetEmployeeById_ReturnsOkResult_WhenEmployeeExists()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var employee = new Employee { Id = employeeId, Name = "John Doe" };
            _mockRepo.Stub(repo => repo.GetByIdAsync(employeeId)).Return(Task.FromResult(employee));

            // Act
            var result = await _controller.GetEmployeeById(employeeId);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnValue = okResult.Value as Employee;
            Assert.AreEqual(employeeId, returnValue.Id);
        }

        [TestMethod]
        public async Task CreateEmployee_ReturnsCreatedResult_WithNewEmployee()
        {
            // Arrange
            var newEmployee = new Employee { Name = "John Doe", Email = "john.doe@example.com" };
            var createdEmployee = new Employee { Id = Guid.NewGuid(), Name = "John Doe", Email = "john.doe@example.com" };
            _mockRepo.Stub(repo => repo.CreateAsync(newEmployee)).Return(Task.FromResult(createdEmployee));

            // Act
            var result = await _controller.CreateEmployee(newEmployee);

            // Assert
            var createdResult = result.Result as CreatedAtActionResult;
            Assert.IsNotNull(createdResult);
            var returnValue = createdResult.Value as Employee;
            Assert.AreEqual(createdEmployee.Id, returnValue.Id);
        }

        [TestMethod]
        public async Task UpdateEmployee_ReturnsOkResult_WhenUpdateIsSuccessful()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var updatedEmployee = new Employee { Id = employeeId, Name = "John Doe", Email = "john.doe@example.com" };
            _mockRepo.Stub(repo => repo.UpdateAsync(updatedEmployee)).Return(Task.FromResult(updatedEmployee));

            // Act
            var result = await _controller.UpdateEmployee(employeeId, updatedEmployee);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public async Task DeleteEmployee_ReturnsOkResult_WhenDeleteIsSuccessful()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            _mockRepo.Stub(repo => repo.DeleteAsync(employeeId)).Return(Task.FromResult(true));

            // Act
            var result = await _controller.DeleteEmployee(employeeId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }
    }
}
