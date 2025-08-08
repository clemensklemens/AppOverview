using AppOverview.Components;
using AppOverview.Model.Interfaces;
using AppOverview.Service;
using NLog;
using NLog.Web;

namespace AppOverview
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Setup NLog for Dependency injection
            var loggerSetup = NLog.LogManager.Setup().LoadConfigurationFromAppSettings();
            var logger = NLog.LogManager.GetCurrentClassLogger();
            try
            {
                //get connection string from appsettings.json
                var config = new ConfigurationBuilder()
                    .AddJsonFile($"appsettings.json", true, true)
                    .Build();
                if (config is null)
                {
                    throw new InvalidOperationException("Configuration could not be loaded. Ensure appsettings.json exists and is properly formatted.");
                }
                string? connectionString = config.GetConnectionString("DefaultConnection");
                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    throw new InvalidOperationException("Connection string is missing or empty in appsettings.json.");
                }

                // Ensure SQLite DB path is absolute and in the same folder as the executable
                if (connectionString.EndsWith(".db") && !connectionString.StartsWith("Data Source"))
                {
                    string exePath = AppContext.BaseDirectory;
                    connectionString = "Data Source=" + Path.Combine(exePath, connectionString);

                    var context = new AppOverview.Data.Models.AppOverviewContext(connectionString);
                    context.Database.EnsureCreated();
                }

                var builder = WebApplication.CreateBuilder(args);

                // Remove default logging providers and add NLog
                builder.Logging.ClearProviders();
                builder.Host.UseNLog();

                // Add services to the container.
                builder.Services.AddRazorComponents()
                    .AddInteractiveServerComponents()
                    .AddInteractiveWebAssemblyComponents();

                // Register your application services
                builder.Services.AddSingleton<IDataProvider>(p => new Data.DataProvider(connectionString));
                builder.Services.AddSingleton<ITechnologyService, TechnologyService>();
                builder.Services.AddSingleton<IDepartmentService, DepartmentService>();
                builder.Services.AddSingleton<IEntityService, EntityService>();
                builder.Services.AddSingleton<IEntityTypeService, EntityTypeService>();
                builder.Services.AddSingleton<IEntityRelationsService, EntityRelationsService>();
                if (OperatingSystem.IsWindows())
                {
                    // Add Windows Authentication (Active Directory)
                    builder.Services.AddAuthentication(Microsoft.AspNetCore.Server.IISIntegration.IISDefaults.AuthenticationScheme);
                    builder.Services.AddSingleton<IUserAuthService, UserAuthServiceWindows>();
                }
                else if (OperatingSystem.IsLinux())
                {
                    builder.Services.AddSingleton<IUserAuthService, UserAuthServiceLinux>();
                }
                else
                {
                    throw new PlatformNotSupportedException("Unsupported operating system for user authentication.");
                }

                var app = builder.Build();

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.UseWebAssemblyDebugging();
                }
                else
                {
                    app.UseExceptionHandler("/Error");
                    app.UseHsts();
                }

                app.UseHttpsRedirection();
                app.UseStaticFiles();
                app.UseAntiforgery();

                // Enable authentication middleware
                app.UseAuthentication();
                app.UseAuthorization();

                app.MapRazorComponents<App>()
                    .AddInteractiveServerRenderMode()
                    .AddInteractiveWebAssemblyRenderMode();

                app.Run();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Stopped program because of exception");
                Console.Error.WriteLine($"Exception: {ex}");
            }
            finally
            {
                LogManager.Shutdown();
            }
        }
    }
}
