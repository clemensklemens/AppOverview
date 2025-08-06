// Code-behind for Technologies.razor
using AppOverview.Model.DTOs;
using Microsoft.AspNetCore.Components;

namespace AppOverview.Components.Pages
{
    public partial class Technologies : ComponentBase
    {
        private List<TechnologyDTO>? _technologies;
        private TechnologyDTO _editTechnology = new TechnologyDTO();
        private bool _showForm = false;
        private bool _isEdit = false;
        protected bool _nameInvalid = false;

        protected override async Task OnInitializedAsync()
        {            
            var data = await Service.GetTechnologiesAsync();
            _technologies = data.ToList();
        }

        private void ShowNewForm()
        {
            _editTechnology = new TechnologyDTO();
            _showForm = true;
            _isEdit = false;
        }

        private void ShowEditForm(TechnologyDTO technology)
        {
            _editTechnology = technology;
            _showForm = true;
            _isEdit = true;
        }

        private void CancelEdit()
        {
            _showForm = false;
        }

        private async Task OnSubmitTechAsync()
        {
            if (_technologies == null)
            {
                return;
            }

            // Validate name
            if (string.IsNullOrWhiteSpace(_editTechnology.Name))
            {
                _nameInvalid = true;
                StateHasChanged();
                return;
            }
            _nameInvalid = false;

            if (_isEdit)
            {
                await Service.UpdateTechnologyAsync(_editTechnology);
                var idx = _technologies.FindIndex(t => t.Id == _editTechnology.Id);
                if (idx >= 0)
                {
                    _technologies[idx] = _editTechnology;
                }            
            }
            else
            {
                var newTechnology = await Service.AddTechnologyAsync(_editTechnology);
                _technologies.Add(newTechnology);
            }
            _showForm = false;
            StateHasChanged();
        }
    }
}
