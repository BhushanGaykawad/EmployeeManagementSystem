using EmployeeManagementSystem.Model;

namespace EmployeeManagementSystem.Repository
{
    public interface IDeparment
    {
        Task<IEnumerable<Department>> GetAllDeparments();
        Task<Department> GetDeparmentsById(int departmentId);
        Task<Department> CreateDepartment(Department department);
        Task<Department> UpdateDepartment(Department department);   
        Task<bool> DeleteDepartment(int departmentId);
    }
}
