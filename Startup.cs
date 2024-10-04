using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Repository;
using EmployeeManagementSystem.RepositoryImpl;
using EmployeeManagementSystem.RepositoryImplementations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace EmployeeManagementSystem
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        // This method is called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = _configuration.GetConnectionString("DBConnection");

            // Register ApplicationDbContext with SQL Server (replace with your DB provider)
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString)); 
            services.AddControllers(); // Register MVC controllers
            services.AddEndpointsApiExplorer(); // Enable API exploration
            services.AddSwaggerGen(); // Add Swagger for API documentation
            services.AddScoped<IRole,RoleRepositoryImpl>();
            services.AddScoped<IDeparment,DepartmentRepositoryImpl>();
            services.AddScoped<IEmployee,EmployeeRepositoryImpl>();
            services.AddScoped<ISuperUser,SuperUserRepositoryImpl>();
            services.AddScoped<IBlackListedToken, BlackListedTokenImplementation>();

            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = _configuration["Jwt:Issuer"],
                        ValidAudience = _configuration["JWT:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                    };
                    // Set up events for the JWT Bearer authentication
                    options.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = async context =>
                        {
                            // Resolve the blacklisted token repository from the service provider
                            var tokenBlackListedRepo = context.HttpContext.RequestServices.GetRequiredService<IBlackListedToken>();

                            // Get the token from the authorization header
                            var token = context.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                            // Check if the token is blacklisted
                            if (await tokenBlackListedRepo.IsTokenBlackListed(token))
                            {
                                context.Fail("Token is blacklisted.");
                            }
                        }
                    };
                });
            services.AddAuthorization();
            services.AddControllers();

        }

        // This method is called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage(); // Show developer exception page
                app.UseSwagger(); // Enable Swagger in development
                app.UseSwaggerUI(); // Swagger UI setup
            }
            else
            {
                app.UseExceptionHandler("/Home/Error"); // Handle errors in production
                app.UseHsts(); // Use HTTP Strict Transport Security
            }
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.Migrate(); // Apply migrations

                var superUserRepository = scope.ServiceProvider.GetRequiredService<ISuperUser>();
                superUserRepository.SeedSuperUserAsync().GetAwaiter().GetResult(); // Seed Super User
            }

            app.UseHttpsRedirection(); // Redirect HTTP to HTTPS
            app.UseRouting(); // Enable routing
            app.UseAuthentication();
            app.UseAuthorization(); // Enable authorization

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers(); // Map attribute-based routes to controllers
            });
        }
    }
}
