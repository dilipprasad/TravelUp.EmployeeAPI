using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelUp.EmployeeAPI.Data.Models
{
    public class Employee
    {
        [Key]
        public Guid Id { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; } = string.Empty;

        [Column(TypeName = "nvarchar(200)")]
        public string Email { get; set; } = string.Empty;

        [Column(TypeName = "nvarchar(2000)")]
        public string Address { get; set; } = string.Empty;
    }
}
