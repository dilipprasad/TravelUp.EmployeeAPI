using Microsoft.AspNetCore.Mvc;
using TravelUp.EmployeeAPI.Data.Exceptions;
using TravelUp.EmployeeAPI.Data.Models;
using TravelUp.EmployeeAPI.Data.Repository;

namespace TravelUp.EmployeeAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<EmployeeController> _logger;
        private readonly string _logTitle = "TravelUp.EmployeeAPI.Controllers.EmployeeController ";

        public EmployeeController(IEmployeeRepository employeeRepository,ILogger<EmployeeController> logger)
        {
            _employeeRepository = employeeRepository;
            this._logger = logger;
        }

        /// <summary>
        /// Fetches all the Employees
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)] // For a successful response
        [ProducesResponseType(StatusCodes.Status500InternalServerError)] // For Problem()

        public async Task<ActionResult<IEnumerable<Employee>>> GetAllEmployees()
        {
            try
            {
                var employees = await _employeeRepository.GetAllAsync();
                return Ok(employees);
            }
            catch (RepositoryException ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching all employees");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Problem in {_logTitle}GetAllEmployees",ex);
                return Problem("Problem Fetching GetAllEmployees");
            }
            
        }

        /// <summary>
        /// Get paginated employee records.
        /// Example: GET /api/employee/paginated?pageNumber=1&pageSize=10
        /// </summary>
        [HttpGet("GetPaginated")]
        [ProducesResponseType(StatusCodes.Status200OK)] // For a successful response
        [ProducesResponseType(StatusCodes.Status500InternalServerError)] // For Problem()
        public async Task<ActionResult<PaginatedResult<Employee>>> GetPaginated(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var result = await _employeeRepository.GetPaginatedAsync(pageNumber, pageSize);
                return Ok(result);
            }
            catch (RepositoryException ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching employees");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Problem in {_logTitle}GetPaginated", ex);
                return Problem("Problem fetching paginated employees");
            }
        }

        /// <summary>
        /// Fetches Employee By Employee ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)] // For a successful response
        [ProducesResponseType(StatusCodes.Status500InternalServerError)] // For Problem()
        public async Task<ActionResult<Employee>> GetEmployeeById(Guid id)
        {
            try
            {
                var employee = await _employeeRepository.GetByIdAsync(id);
                if (employee == null)
                    return Problem("Unable to Find Employee By Id");

                return Ok(employee);
            }
            catch (RepositoryException ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching employee with ID: {id}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Problem in {_logTitle}GetEmployeeById", ex);
                return Problem("Problem fetching employee by Id");
            }
        }

        /// <summary>
        /// Create a new employee
        /// </summary>
        /// <param name="newEmployee">Employee Information to Create</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)] // For a successful response
        [ProducesResponseType(StatusCodes.Status500InternalServerError)] // For Problem()
        public async Task<ActionResult<Employee>> CreateEmployee([FromBody] Employee newEmployee)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(newEmployee.Name))
                    return BadRequest("Employee Name cannot be empty.");

                if (string.IsNullOrWhiteSpace(newEmployee.Email))
                    return BadRequest("Employee Email cannot be empty.");

                var createdEmployee = await _employeeRepository.CreateAsync(newEmployee);

                // Return the newly created resource, adhering to REST conventions
                return CreatedAtAction(nameof(GetEmployeeById), new { id = createdEmployee.Id }, createdEmployee);
            }
            catch (RepositoryException ex)
            {
                _logger.LogError(ex, $"An error occurred while creating employee");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Problem in {_logTitle}CreateEmployee", ex);
                return Problem("Problem creating a new employee");
            }
        }

        /// <summary>
        /// Update the Employee 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updatedEmployee"></param>
        /// <returns></returns>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)] // For a successful response
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // For a Bad Request
        [ProducesResponseType(StatusCodes.Status500InternalServerError)] // For Problem()
        public async Task<IActionResult> UpdateEmployee(Guid id, [FromBody] Employee updatedEmployee)
        {
            try
            {
                if (id == Guid.Empty)
                    return BadRequest("Invalid employee ID.");

                // Ensure the Employee object's ID matches the route ID
                updatedEmployee.Id = id;

                var updated = await _employeeRepository.UpdateAsync(updatedEmployee);
                if (updated == null)
                    return Problem("Problem updating employee.");

                return Ok();
            }
            catch (RepositoryException ex)
            {
                _logger.LogError(ex, $"An error occurred while updating employee with ID: {id}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Problem in {_logTitle}UpdateEmployee", ex);
                return Problem("Problem occurred while updating the employee.");
            }
        }

        /// <summary>
        /// Delete the Employee by Employee ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)] // For a successful response
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // For a Bad Request
        [ProducesResponseType(StatusCodes.Status500InternalServerError)] // For Problem()
        public async Task<IActionResult> DeleteEmployee(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return BadRequest("Invalid employee ID.");

                var success = await _employeeRepository.DeleteAsync(id);
                if (!success)
                    return Problem("Problem deleting employee.");

                return Ok();
            }
            catch (RepositoryException ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting employee with ID: {id}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Problem in {_logTitle}DeleteEmployee", ex);
                return Problem("Problem occurred while deleting the employee.");
            }
        }

    }
}
