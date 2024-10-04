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

namespace EmployeeManagementSystem.RepositoryImpl
{
    public class SuperUserRepositoryImpl : ISuperUser
    {
        public readonly ApplicationDbContext _context;
        public readonly IBlackListedToken _blackListedToken;
        
        
        public SuperUserRepositoryImpl(ApplicationDbContext context,IBlackListedToken blackListedToken)
        {
            _context = context;
            _blackListedToken = blackListedToken;
        }
        public async Task<string> Login(string username, string password)
        {
            var user = _context.SuperUserDetails.SingleOrDefault(sa => sa.UserName == username);
            if (user == null || !VerifiedPasswords(password, user.UserPassword))
            {
                return null;
            }
            return GenerateJWTToken(user);
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
    }
}
