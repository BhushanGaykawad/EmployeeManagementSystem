using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.DTO;
using EmployeeManagementSystem.Model;
using EmployeeManagementSystem.Repository;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace EmployeeManagementSystem.RepositoryImplementations
{
    public class EmployeeRepositoryImpl : IEmployee
    {
        private readonly ApplicationDbContext _context;
        public EmployeeRepositoryImpl(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Employee> CreateEmployee(Employee employee)
        {
            await _context.Employee.AddAsync(employee);
            await _context.SaveChangesAsync();
            return employee;
        }

        public async Task<bool> DeleteEmployee(int employeeid)
        {
            var employee=await _context.Employee.FindAsync(employeeid);
            if (employee == null)
            {
                return false;
            }
            _context.Employee.Remove(employee);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Employee>> GetAllEmployee()
        {
            return await _context.Employee.ToListAsync();
        }

        public async Task<Employee> GetEmployeebyId(int employeeid)
        {
            var employee = await _context.Employee.FindAsync(employeeid);
            if (employee == null)
            {
                return null;
            }
            return employee;
        }

        public async Task<Employee> UpdateEmployee(int id,EmployeeUpdateDTO employeeUpdateDto)
        {
            var existingEmployee= await _context.Employee.FindAsync(id);
            if(existingEmployee == null) {
                return null;
            }
            existingEmployee.EmployeeName= employeeUpdateDto.EmployeeName ?? existingEmployee.EmployeeName;
            existingEmployee.EmployeeEmail=employeeUpdateDto.EmployeeEmail ?? existingEmployee.EmployeeEmail;
            existingEmployee.EmployeePhoneNumber=employeeUpdateDto.EmployeePhoneNumber ?? existingEmployee.EmployeePhoneNumber;


            if (employeeUpdateDto.EmployeeDepartmentId > 0)
            {
                existingEmployee.EmployeeDepartmentId = employeeUpdateDto.EmployeeDepartmentId;
            }

            if (employeeUpdateDto.EmployeeSalary > 0)
            {
                existingEmployee.EmployeeSalary = employeeUpdateDto.EmployeeSalary;
            }

            if (employeeUpdateDto.EmployeeRoleId > 0)
            {
                existingEmployee.EmployeeRoleId = employeeUpdateDto.EmployeeRoleId;
            }

            if (employeeUpdateDto.DateOfJoininig != default)
            {
                existingEmployee.DateOfJoininig = employeeUpdateDto.DateOfJoininig;
            }
            _context.Employee.Update(existingEmployee);
            await _context.SaveChangesAsync();
            return existingEmployee;
        }
    }
}
