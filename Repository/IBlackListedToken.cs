namespace EmployeeManagementSystem.Repository
{
    public interface IBlackListedToken
    {
        Task AddTokenToBlackListAsync(String token);
        Task<bool> IsTokenBlackListed(String token);


    }
}
