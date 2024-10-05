using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Repository;
using Microsoft.AspNetCore.Identity;

namespace EmployeeManagementSystem.RepositoryImpl
{
    public class UserAutorizationTokenImpl : IUserAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UserAutorizationTokenImpl> _logger;

        public UserAutorizationTokenImpl(ApplicationDbContext context, ILogger<UserAutorizationTokenImpl> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task StoreTokensAsync(int userId, string accessToken, string refreshToken)
        {
            _logger.LogInformation($"Token received in storeTokenAsync is:{userId},{accessToken},{refreshToken}");
            var userTokens = new Model.UserAuthorizationToken
            {
                UserId = userId,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddMinutes(30) // Set expiry time as needed
            };

            await _context.UserTokens.AddAsync(userTokens);
            await _context.SaveChangesAsync();
        }
    }

}
