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
                var builder = WebApplication.CreateBuilder(args);

                // Remove default logging providers and add NLog
                builder.Logging.ClearProviders();
                builder.Host.UseNLog();

                // Add services to the container.
                builder.Services.AddRazorComponents()
                    .AddInteractiveServerComponents()
                    .AddInteractiveWebAssemblyComponents();

                // Add Windows Authentication (Active Directory)
                builder.Services.AddAuthentication(Microsoft.AspNetCore.Server.IISIntegration.IISDefaults.AuthenticationScheme);

                // Register your application services
                builder.Services.AddSingleton<IDataProvider, Data.DataProvider>();
                builder.Services.AddSingleton<ITechnologyService, TechnologyService>();
                builder.Services.AddSingleton<IDepartmentService, DepartmentService>();
                builder.Services.AddSingleton<IEntityService, EntityService>();
                builder.Services.AddSingleton<IEntityTypeService, EntityTypeService>();
                builder.Services.AddSingleton<IEntityRelationsService, EntityRelationsService>();
                if (OperatingSystem.IsWindows())
                {
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
