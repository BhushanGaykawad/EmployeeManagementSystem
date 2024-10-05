using EmployeeManagementSystem.Repository;

using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Model;
using EmployeeManagementSystem.Repository;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Azure.Core;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EmployeeManagementSystem.RepositoryImpl
{
    public class SuperUserRepositoryImpl : ISuperUser
    {
        public readonly ApplicationDbContext _context;
        public readonly IBlackListedToken _blackListedToken;
        public readonly ILogger<ISuperUser> _logger;
        private readonly IUserAuthService  _tokenService;


        public SuperUserRepositoryImpl(ApplicationDbContext context,IBlackListedToken blackListedToken,ILogger<ISuperUser> logger,IUserAuthService tokenService)
        {
            _context = context;
            _blackListedToken = blackListedToken;
            _logger = logger;
            _tokenService = tokenService;
        }
   

        private string GenerateRefreshToken()
        {
            // Generate a random string for the refresh token
            using (var rng = new RNGCryptoServiceProvider())
            {
                var tokenData = new byte[32];
                rng.GetBytes(tokenData);
                return Convert.ToBase64String(tokenData);
            }
        }

        public async Task SeedSuperUserAsync()
        {
           if(!await _context.SuperUserDetails.AnyAsync())
            {
                var superUser = new SuperUserDetails
                {
                    //SuperUserId = 1,
                    UserName = "superadmin",
                    UserPassword = CreateHashedPassword("super@admin")
                };
                await _context.SuperUserDetails.AddAsync(superUser);
                await _context.SaveChangesAsync();
            }
        }


        private string CreateHashedPassword(string password)
        {
            using(var sha256=SHA256.Create())
            {
                var hashedCode=sha256.ComputeHash(Encoding.UTF8.GetBytes(password));    
                return BitConverter.ToString(hashedCode).Replace("_","").ToLower();
            }
        }
        private bool VerifiedPasswords(string password, string userPassword)
        {
            var hashedPassword=CreateHashedPassword(password);
            return hashedPassword==userPassword;
        }

        public string GenerateJWTToken(SuperUserDetails user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("A1b2C3d4E5f6G7h8I9j0K1l2M3n4O5p6"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "EmployeeManagementAdmin",
                audience: "EmployeeManagementApp",
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> Logout(string token)
        {
            await _blackListedToken.AddTokenToBlackListAsync(token);
            return "Successfully Logout.";
        }
        public async Task<(string newAccessToken,string newRefreshToken)>RefreshToken(String expiredToken,string refreshToken)
        {
            _logger.LogInformation($"Expired token received is {expiredToken} and refresh token received is{refreshToken}");
            await _blackListedToken.AddTokenToBlackListAsync(expiredToken);
            

            var user=await _context.SuperUserDetails.SingleOrDefaultAsync(u=>u.RefreshToken==refreshToken);
            if (user==null || user.RefreshTokenExpiry <DateTime.UtcNow)
            {
                
                throw new UnauthorizedAccessException("Invalid or Expired refresh token");

            }
            var newAccessToken=GenerateJWTToken(user);
            var newRefreshToken = Guid.NewGuid().ToString();

            user.RefreshToken=newRefreshToken;
            user.RefreshTokenExpiry=DateTime.UtcNow.AddHours(30);
            await _context.SaveChangesAsync();
            return(newAccessToken,newRefreshToken);
         
        }

        public async Task<(string newAccessToken, string newRefreshToken)> LogIn(string username, string password)
        {

            var user = await _context.SuperUserDetails.SingleOrDefaultAsync(sa => sa.UserName == username);
            if (user == null || !VerifiedPasswords(password, user.UserPassword))
            {
                return (null, null);
            }

            var accessToken = GenerateJWTToken(user);
            var refreshToken = GenerateRefreshToken();

            _logger.LogInformation($"Generated Access Token: {accessToken}");
            _logger.LogInformation($"Generated Refresh Token: {refreshToken}");

            user.AccessToken = accessToken;
            user.RefreshToken = refreshToken; // Update refresh token
            user.RefreshTokenExpiry = DateTime.UtcNow.AddHours(30);

            _logger.LogInformation($"User before update: {user.UserName}, {user.AccessToken}, {user.RefreshToken}");

            _context.SuperUserDetails.Update(user);
            await _context.SaveChangesAsync();

            try
            {
                await _tokenService.StoreTokensAsync(user.SuperUserId, accessToken, refreshToken);
                _logger.LogInformation("Tokens updated successfully in the database");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error storing tokens: {ex.Message}");
            }

            _logger.LogInformation("Tokens updated successfully in the database");

            return (accessToken, refreshToken);
        }
    }
}
