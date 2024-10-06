using EmployeeManagementSystem.DTO;
using EmployeeManagementSystem.Model;

namespace EmployeeManagementSystem.Repository
{
    public interface IEmployee
    {
        //Task<IEnumerable<Employee>> GetAllEmployee();
        Task<IEnumerable<EmployeeDTO>> GetAllEmployeeWithDetails(); // New method for DTO

        Task<Employee> GetEmployeebyId(int employeeid);
        Task<Employee> CreateEmployee(Employee employee);   
        Task<Employee> UpdateEmployee(int id,EmployeeUpdateDTO employeeUpdateDto);   
        Task<bool> DeleteEmployee(int employeeid);
    }
}
