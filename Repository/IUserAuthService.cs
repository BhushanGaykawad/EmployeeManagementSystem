namespace EmployeeManagementSystem.Repository
{
    public interface IUserAuthService
    {
        Task StoreTokensAsync(int superuserId, string accessToken, string refreshToken);

    }
}
