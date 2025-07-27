// Code-behind for Departments.razor
using AppOverview.Model;
using Microsoft.AspNetCore.Components;

namespace AppOverview.Components.Pages
{
    public partial class Departments : ComponentBase
    {
        private List<DepartmentDTO>? _departments;
        private DepartmentDTO _editDepartment = new DepartmentDTO();
        private bool _showForm = false;
        private bool _isEdit = false;

        protected override async Task OnInitializedAsync()
        {
            var data = await Service.GetAllDepartmentsAsync();
            _departments = data.ToList();
        }

        private void ShowNewForm()
        {
            _editDepartment = new DepartmentDTO();
            _showForm = true;
            _isEdit = false;
        }

        private void ShowEditForm(DepartmentDTO department)
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

            if (_isEdit)
            {
                await Service.UpdateDepartmentAsync(_editDepartment);
                var idx = _departments.FindIndex(d => d.Id == _editDepartment.Id);
                if (idx >= 0)
                {
                    _departments[idx] = _editDepartment;                    
                }
            }
            else
            {
                var newDepartment = await Service.AddDepartmentAsync(_editDepartment);
                _departments.Add(newDepartment);                
            }
            _showForm = false;
            StateHasChanged();
        }
    }
}
