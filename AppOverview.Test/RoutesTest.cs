using AppOverview.Components;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace AppOverview.Test
{
    public class RoutesErrorHandlingTest : TestContext
    {
        [Fact]
        public void NotFound_ShowsNotFoundMessage()
        {
            var cut = RenderComponent<Routes>();
            var navMan = Services.GetRequiredService<NavigationManager>();
            navMan.NavigateTo("/non-existent-page");
            cut.Render();
            Assert.Contains("Sorry, there's nothing at this address.", cut.Markup);
        }

        [Fact]
        public void ErrorBoundary_ShowsErrorContent()
        {
            Services.AddSingleton<TestExceptionComponent>();
            var cut = RenderComponent<Routes>();
            Assert.Contains("An unexpected error occurred. Please try again later.", cut.Markup);
        }
    }

    public class TestExceptionComponent : Microsoft.AspNetCore.Components.ComponentBase
    {
        protected override void OnInitialized() => throw new System.InvalidOperationException();
    }
}