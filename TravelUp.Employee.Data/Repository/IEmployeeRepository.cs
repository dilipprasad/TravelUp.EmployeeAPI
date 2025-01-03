using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelUp.EmployeeAPI.Data.Models;

namespace TravelUp.EmployeeAPI.Data.Repository
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetAllAsync();
        Task<Employee?> GetByIdAsync(Guid id);
        Task<Employee> CreateAsync(Employee employee);
        Task<Employee?> UpdateAsync(Employee employee);
        Task<bool> DeleteAsync(Guid id);

        // Add the pagination method
        Task<PaginatedResult<Employee>> GetPaginatedAsync(int pageNumber, int pageSize);
    }
}
