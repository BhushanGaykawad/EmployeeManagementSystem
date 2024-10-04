using EmployeeManagementSystem.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagementSystem.DTO
{
    public class EmployeeUpdateDTO
    {
        public String EmployeeName {  get; set; }
        public String EmployeeEmail { get; set; }   
        public string EmployeePhoneNumber { get; set; }
        public int EmployeeDepartmentId { get; set; }
        public int EmployeeRoleId { get; set; }
        public DateTime DateOfJoininig { get; set; }
        public float  EmployeeSalary { get; set; }
    }
}
