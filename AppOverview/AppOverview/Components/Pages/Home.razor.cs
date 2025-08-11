// Code-behind for Technologies.razor
using AppOverview.Model;
using AppOverview.Model.DTOs;
using Microsoft.AspNetCore.Components;

namespace AppOverview.Components.Pages
{
    public partial class Home : ComponentBase
    {
        private User? _currentUser;
        private string? _errorMessage;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                _currentUser = UserService.GetUserNameAndPermissions();
            }
            catch (Exception ex)
            {
                _errorMessage = $"Error getting user permissions: {ex.Message}";
            }
        }
    }
}
