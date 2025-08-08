// Code-behind for Departments.razor
using AppOverview.Model;
using AppOverview.Model.DTOs;
using Microsoft.AspNetCore.Components;

namespace AppOverview.Components.Pages
{
    public partial class Departments : ComponentBase
    {
        private List<IdNameDTO>? _departments;
        private IdNameDTO _editDepartment = new IdNameDTO();
        private bool _showForm = false;
        private bool _isEdit = false;
        private bool _nameInvalid = false;
        private User? _currentUser;
        private string? _errorMessage;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                _currentUser = UserService.GetUserNameAndPermissions();
                _departments = (await Service.GetDepartmentsAsync()).ToList();                
            }
            catch (Exception ex)
            {
                _errorMessage = $"Error loading departments: {ex.Message}";
            }
        }

        private void ShowNewForm()
        {
            _editDepartment = new IdNameDTO();
            _showForm = true;
            _isEdit = false;
        }

        private void ShowEditForm(IdNameDTO department)
        {
            _editDepartment = department;
            _showForm = true;
            _isEdit = true;
        }

        private void CancelEdit()
        {
            _showForm = false;
        }

        private async Task OnSubmitDepartmentAsync()
        {
            if (_departments == null)
            {
                return;
            }

            // Validate name
            if (string.IsNullOrWhiteSpace(_editDepartment.Name))
            {
                _nameInvalid = true;
                StateHasChanged();
                return;
            }
            _nameInvalid = false;

            try
            {
                if (_isEdit)
                {
                    await Service.UpdateDepartmentAsync(_editDepartment, _currentUser?.Name??string.Empty);
                    var idx = _departments.FindIndex(d => d.Id == _editDepartment.Id);
                    if (idx >= 0)
                    {
                        _departments[idx] = _editDepartment;                    
                    }
                }
                else
                {
                    var newDepartment = await Service.AddDepartmentAsync(_editDepartment, _currentUser?.Name ?? string.Empty);
                    _departments.Add(newDepartment);                
                }
                _showForm = false;
            }
            catch (Exception ex)
            {
                _errorMessage = $"Error saving department: {ex.Message}";
            }
            StateHasChanged();
        }
    }
}
