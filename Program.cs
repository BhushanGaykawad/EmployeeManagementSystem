using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace EmployeeManagementSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Configure Serilog
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("EmployeeManagementSystem/log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            try
            {
                Log.Information("Starting the Employee Management Web Host");
                CreateHostBuilder(args).Build().Run(); // Start the web host
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Employee Management Application Startup Failed"); // Log fatal error with exception details
            }
            finally
            {
                Log.CloseAndFlush(); // Clean up log resources
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog() // Use Serilog for logging
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>(); // Reference the Startup class for configuration
                });
    }
}
