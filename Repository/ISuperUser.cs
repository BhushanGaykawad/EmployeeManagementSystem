using EmployeeManagementSystem.Model;

namespace EmployeeManagementSystem.Repository
{
    public interface ISuperUser
    {
        Task<(string newAccessToken, string newRefreshToken)> LogIn(string username, string password);
        Task SeedSuperUserAsync();
        Task<string> Logout(string token);
        Task<(string newAccessToken, string newRefreshToken)> RefreshToken(string token, string refreshToken);
    }
}
