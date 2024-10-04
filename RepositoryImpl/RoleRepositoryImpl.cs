using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Model;
using EmployeeManagementSystem.Repository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.RepositoryImplementations
{
    public class RoleRepositoryImpl : IRole
    {
        private readonly ApplicationDbContext _context;

        public RoleRepositoryImpl(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Role> CreateRole(Role role)
        {
            await _context.Role.AddAsync(role);
            await _context.SaveChangesAsync();
            return role;
        }

        public async Task<Role> UpdateRole(Role role)
        {
            var existingRole = await _context.Role.FindAsync(role.RoleID);
            if (existingRole == null)
            {
                return null;
            }

            existingRole.RoleType = role.RoleType; 

            await _context.SaveChangesAsync();

            return existingRole;
        }


        public async Task<bool> DeleteRole(int roleId)
        {
            var role = await _context.Role.FindAsync(roleId);
            if (role == null)
            {
                return false;
            }
            _context.Role.Remove(role);
            await _context.SaveChangesAsync(); 
            return true;
        }

        public async Task<IEnumerable<Role>> GetAllRoles()
        {
            return await _context.Role.ToListAsync();
        }

        public async Task<Role> GetRoleById(int roleId)
        {
            var role = await _context.Role.FindAsync(roleId);
            return (role);
        }
    }
}
