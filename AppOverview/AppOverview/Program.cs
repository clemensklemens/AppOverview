using AppOverview.Client.Pages;
using AppOverview.Components;
using AppOverview.Components.Account;
using AppOverview.Data;
using AppOverview.Model.Interfaces;
using AppOverview.Service;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
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

                builder.Services.AddCascadingAuthenticationState();
                builder.Services.AddScoped<IdentityUserAccessor>();
                builder.Services.AddScoped<IdentityRedirectManager>();
                builder.Services.AddScoped<AuthenticationStateProvider, PersistingRevalidatingAuthenticationStateProvider>();

                builder.Services.AddAuthentication(options =>
                    {
                        options.DefaultScheme = IdentityConstants.ApplicationScheme;
                        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
                    })
                    .AddIdentityCookies();

                var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
                builder.Services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(connectionString));
                builder.Services.AddDatabaseDeveloperPageExceptionFilter();

                builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddSignInManager()
                    .AddDefaultTokenProviders();

                builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();
                builder.Services.AddSingleton<IDataProvider, DataProvider>();
                builder.Services.AddSingleton<ITechnologyService, TechnologyService>();
                builder.Services.AddSingleton<IDepartmentService, DepartmentService>();
                builder.Services.AddSingleton<IEntityService, EntityService>();
                builder.Services.AddSingleton<IEntityTypeService, EntityTypeService>();
                builder.Services.AddSingleton<IEntityRelationsService, EntityRelationsService>();

                var app = builder.Build();

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.UseWebAssemblyDebugging();
                    app.UseMigrationsEndPoint();
                }
                else
                {
                    app.UseExceptionHandler("/Error");
                    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                    app.UseHsts();
                }

                app.UseHttpsRedirection();

                app.UseStaticFiles();
                app.UseAntiforgery();

                app.MapRazorComponents<App>()
                    .AddInteractiveServerRenderMode()
                    .AddInteractiveWebAssemblyRenderMode()
                    .AddAdditionalAssemblies(typeof(Client._Imports).Assembly);

                // Add additional endpoints required by the Identity /Account Razor components.
                app.MapAdditionalIdentityEndpoints();

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
