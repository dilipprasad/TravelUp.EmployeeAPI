using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TravelUp.EmployeeAPI.Data.Exceptions;
using TravelUp.EmployeeAPI.Data.Models;

namespace TravelUp.EmployeeAPI.Data.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EmployeeRepository> _logger;

        public EmployeeRepository(ApplicationDbContext context, ILogger<EmployeeRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            try
            {
                return await _context.Employees.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all employees.");
                throw new RepositoryException("Failed to fetch all employees.", ex);
            }
        }

        public async Task<PaginatedResult<Employee>> GetPaginatedAsync(int pageNumber, int pageSize)
        {
            try
            {
                // Ensure valid values
                if (pageNumber < 1) pageNumber = 1;
                if (pageSize < 1) pageSize = 10;

                var totalCount = await _context.Employees.CountAsync();

                // Calculate how many items to skip
                var skip = (pageNumber - 1) * pageSize;

                // Fetch the current page
                var employees = await _context.Employees
                    .OrderBy(e => e.Name) // Example: sort by Name
                    .Skip(skip)
                    .Take(pageSize)
                    .ToListAsync();

                return new PaginatedResult<Employee>(employees, totalCount, pageNumber, pageSize);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching paginated employees for page {pageNumber} and size {pageSize}.");
                throw new RepositoryException("Failed to fetch paginated employees.", ex);
            }
        }

        public async Task<Employee?> GetByIdAsync(Guid id)
        {
            try
            {
                return await _context.Employees.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching employee by ID: {id}.");
                throw new RepositoryException("Failed to fetch employee by ID.", ex);
            }
        }

        public async Task<Employee> CreateAsync(Employee employee)
        {
            try
            {
                employee.Id = Guid.NewGuid();

                await _context.Employees.AddAsync(employee);
                await _context.SaveChangesAsync();

                return employee;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new employee.");
                throw new RepositoryException("Failed to create employee.", ex);
            }
        }

        public async Task<Employee?> UpdateAsync(Employee employee)
        {
            try
            {
                var existing = await _context.Employees.FindAsync(employee.Id);
                if (existing == null) return null;

                existing.Name = employee.Name;
                existing.Email = employee.Email;
                existing.Address = employee.Address;

                _context.Employees.Update(existing);
                await _context.SaveChangesAsync();

                return existing;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating employee with ID: {employee.Id}.");
                throw new RepositoryException("Failed to update employee.", ex);
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                var existing = await _context.Employees.FindAsync(id);
                if (existing == null) return false;

                _context.Employees.Remove(existing);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting employee with ID: {id}.");
                throw new RepositoryException("Failed to delete employee.", ex);
            }
        }
    }

}
