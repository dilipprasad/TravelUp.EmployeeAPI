using Microsoft.EntityFrameworkCore;
using TravelUp.EmployeeAPI.Data.Models;

namespace TravelUp.EmployeeAPI.Data.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;

        public EmployeeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _context.Employees.ToListAsync();
        }

        public async Task<PaginatedResult<Employee>> GetPaginatedAsync(int pageNumber, int pageSize)
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

        public async Task<Employee?> GetByIdAsync(Guid id)
        {
            return await _context.Employees.FindAsync(id);
        }

        public async Task<Employee> CreateAsync(Employee employee)
        {
            // Assign a new GUID if none is provided
            employee.Id = Guid.NewGuid();

            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();

            return employee;
        }

        public async Task<Employee?> UpdateAsync(Employee employee)
        {
            // Check if the entity exists
            var existing = await _context.Employees.FindAsync(employee.Id);
            if (existing == null) return null;

            // Update fields
            existing.Name = employee.Name;
            existing.Email = employee.Email;
            existing.Address = employee.Address;

            _context.Employees.Update(existing);
            await _context.SaveChangesAsync();

            return existing;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var existing = await _context.Employees.FindAsync(id);
            if (existing == null) return false;

            _context.Employees.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
