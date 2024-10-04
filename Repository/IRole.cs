using EmployeeManagementSystem.Model;

namespace EmployeeManagementSystem.Repository
{
    public interface IRole
    {
        Task<IEnumerable<Role>> GetAllRoles();
        Task<Role> GetRoleById(int roleId);
        Task<Role> CreateRole(Role role);
        Task<Role> UpdateRole(Role role);
        Task<bool> DeleteRole(int roleId);
    }
}
