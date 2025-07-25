// Code-behind for Technologies.razor
using AppOverview.Model;
using Microsoft.AspNetCore.Components;

namespace AppOverview.Components.Pages
{
    public partial class Technologies : ComponentBase
    {
        private List<TechnologyDTO>? technologies;
        private TechnologyDTO editTechnology = new TechnologyDTO();
        private bool showForm = false;
        private bool isEdit = false;

        protected override async Task OnInitializedAsync()
        {
            await Task.Delay(300); // Simulate async loading
            var data = await DataProvider.GetTechnologiesAsync();
            technologies = data.ToList();
        }

        private void ShowNewForm()
        {
            editTechnology = new TechnologyDTO();
            showForm = true;
            isEdit = false;
        }

        private void ShowEditForm(TechnologyDTO technology)
        {
            editTechnology = technology;
            showForm = true;
            isEdit = true;
        }

        private void CancelEdit()
        {
            showForm = false;
        }

        private async Task OnSubmitTech()
        {
            if (technologies == null) return;
            if (isEdit)
            {
                var idx = technologies.FindIndex(t => t.Id == editTechnology.Id);
                if (idx >= 0)
                {
                    technologies[idx] = editTechnology;
                    await DataProvider.UpdateTechnologyAsync(editTechnology);
                }            
            }
            else
            {
                var newId = technologies.Count > 0 ? technologies.Max(t => t.Id) + 1 : 1;
                editTechnology.Id = newId;
                technologies.Add(editTechnology);
                await DataProvider.AddTechnologyAsync(editTechnology);
            }
            showForm = false;
            StateHasChanged();
        }
    }
}
