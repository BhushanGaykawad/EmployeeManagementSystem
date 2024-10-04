using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Model;
using EmployeeManagementSystem.Repository;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.RepositoryImplementations
{
    public class DepartmentRepositoryImpl :IDeparment
    {
        private readonly ApplicationDbContext _context;
        public DepartmentRepositoryImpl(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Department> CreateDepartment(Department department)
        {
            await _context.Department.AddAsync(department);
            await _context.SaveChangesAsync();
            return department;
        }

        public async Task<bool> DeleteDepartment(int departmentId)
        {
            var department= await _context.Department.FindAsync(departmentId);
            if(department == null)
            {
                return false;
            }
            _context.Department.Remove(department);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Department>> GetAllDeparments()
        {
            return await _context.Department.ToListAsync();
        }

        public async Task<Department> GetDeparmentsById(int departmentId)
        {
            var department = await _context.Department.FindAsync(departmentId);
            if(department == null)
            {
                return null;
            }
            return department;
        }

        public async Task<Department> UpdateDepartment(Department department)
        {
            var existingDepartment = await _context.Department.FindAsync(department.DepartmentId);
            if(existingDepartment == null)
            {
                return null;
            }

            existingDepartment.DepartmentName = department.DepartmentName;
            await _context.SaveChangesAsync();
            return department;
        }
    }
}
