using AppOverview.Client;
using AppOverview.Data;
using AppOverview.Model;
using AppOverview.Service;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Logging;

namespace AppOverview.Client
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            // Set minimum log level for browser console
            builder.Logging.SetMinimumLevel(LogLevel.Information);

            builder.Services.AddAuthorizationCore();
            builder.Services.AddCascadingAuthenticationState();
            builder.Services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();
            builder.Services.AddSingleton<IDataProvider, DataProvider>();
            builder.Services.AddSingleton<ITechnologyService, TechnologyService>();
            builder.Services.AddSingleton<IDepartmentService, DepartmentService>();
            builder.Services.AddSingleton<IEntityService, EntityService>();
            builder.Services.AddSingleton<IEntityTypeService, EntityTypeService>();

            await builder.Build().RunAsync();
        }
    }
}
