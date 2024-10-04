using EmployeeManagementSystem.Model;

namespace EmployeeManagementSystem.Repository
{
    public interface ISuperUser
    {
        Task<string> Login(string username, string password);
        Task SeedSuperUserAsync();
        Task<string> Logout(string token);
    }
}
