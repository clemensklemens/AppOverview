// Code-behind for Departments.razor
using AppOverview.Model;
using Microsoft.AspNetCore.Components;

namespace AppOverview.Components.Pages
{
    public partial class Departments : ComponentBase
    {
        private List<DepartmentDTO>? departments;
        private DepartmentDTO editDepartment = new DepartmentDTO();
        private bool showForm = false;
        private bool isEdit = false;

        protected override async Task OnInitializedAsync()
        {
            await Task.Delay(300); // Simulate async loading
            var data = await DataProvider.GetDepartmentsAsync();
            departments = data.ToList();
        }

        private void ShowNewForm()
        {
            editDepartment = new DepartmentDTO();
            showForm = true;
            isEdit = false;
        }

        private void ShowEditForm(DepartmentDTO department)
        {
            editDepartment = department;
            showForm = true;
            isEdit = true;
        }

        private void CancelEdit()
        {
            showForm = false;
        }

        private async Task OnSubmitDepartment()
        {
            if (departments == null)
            {
                return;
            }

            if (isEdit)
            {
                var idx = departments.FindIndex(d => d.Id == editDepartment.Id);
                if (idx >= 0)
                {
                    departments[idx] = editDepartment;
                    await DataProvider.UpdateDepartmentAsync(editDepartment);
                }
            }
            else
            {
                var newId = departments.Count > 0 ? departments.Max(d => d.Id) + 1 : 1;
                editDepartment.Id = newId;
                departments.Add(editDepartment);
                await DataProvider.AddDepartmentAsync(editDepartment);
            }
            showForm = false;
            StateHasChanged();
        }
    }
}
