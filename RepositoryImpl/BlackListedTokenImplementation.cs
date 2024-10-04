using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Model;
using EmployeeManagementSystem.Repository;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.RepositoryImpl
{
    public class BlackListedTokenImplementation : IBlackListedToken
    {
        private readonly ApplicationDbContext _context;

        public BlackListedTokenImplementation(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddTokenToBlackListAsync(string token)
        {
            var blackListedToken = new BlackListedJWTToken { Token = token };
            await _context.BlackListedJWTTokens.AddAsync(blackListedToken);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsTokenBlackListed(string token)
        {
            bool result=await _context.BlackListedJWTTokens.AnyAsync(t => t.Token==token );
            return result;

        }
    }
}
