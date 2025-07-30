// Code-behind for EntityTypes.razor
using AppOverview.Model;
using Microsoft.AspNetCore.Components;

namespace AppOverview.Components.Pages
{
    public partial class EntityTypes : ComponentBase
    {
        private List<EntityTypeDTO>? _entityTypes;
        private EntityTypeDTO _editType = new EntityTypeDTO();
        private bool _showForm = false;
        private bool _isEdit = false;
        protected bool _nameInvalid = false;

        protected override async Task OnInitializedAsync()
        {
            var data = await Service.GetEntityTypesAsync();
            _entityTypes = data.ToList();
        }

        private void ShowNewForm()
        {
            _editType = new EntityTypeDTO();
            _showForm = true;
            _isEdit = false;
        }

        private void ShowEditForm(EntityTypeDTO type)
        {
            _editType = type;
            _showForm = true;
            _isEdit = true;
        }

        private void CancelEdit()
        {
            _showForm = false;
        }

        private async Task OnSubmitTypeAsync()
        {
            if (_entityTypes == null)
            {
                return;
            }

            // Validate name
            if (string.IsNullOrWhiteSpace(_editType.Name))
            {
                _nameInvalid = true;
                StateHasChanged();
                return;
            }
            _nameInvalid = false;

            if (_isEdit)
            {
                await Service.UpdateEntityTypeAsync(_editType);
                var idx = _entityTypes.FindIndex(t => t.Id == _editType.Id);
                if (idx >= 0)
                {
                    _entityTypes[idx] = _editType;                    
                }
            }
            else
            {
                var newEntityType = await Service.AddEntityTypeAsync(_editType);
                _entityTypes.Add(newEntityType);                
            }
            _showForm = false;
            StateHasChanged();
        }
    }
}
