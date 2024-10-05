using EmployeeManagementSystem.Model;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {
        }
        public DbSet<Role> Role { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<Employee> Employee { get; set; }

        public DbSet<SuperUserDetails> SuperUserDetails { get;set; }
        public DbSet<BlackListedJWTToken>BlackListedJWTTokens { get; set; }

        public DbSet<UserAuthorizationToken> UserTokens { get; set; } 

    }
}
