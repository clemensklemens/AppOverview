// Code-behind for EntityTypes.razor
using AppOverview.Model;
using AppOverview.Model.DTOs;
using AppOverview.Model.Interfaces;
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
        private User? _currentUser;
        private string? _errorMessage;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                _currentUser = UserService.GetUserNameAndPermissions();
                var data = await Service.GetEntityTypesAsync();
                _entityTypes = data.ToList();
            }
            catch (Exception ex)
            {
                _errorMessage = $"Error loading entity types: {ex.Message}";
            }
        }

        private void ShowNewForm()
        {
            if (_currentUser?.IsAdmin != true) return;
            _editType = new EntityTypeDTO();
            _showForm = true;
            _isEdit = false;
        }

        private void ShowEditForm(EntityTypeDTO type)
        {
            if (_currentUser?.IsAdmin != true) return;
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
            if (_currentUser?.IsAdmin != true) return;
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
            try
            {
                if (_isEdit)
                {
                    await Service.UpdateEntityTypeAsync(_editType, _currentUser?.Name??string.Empty);
                    var idx = _entityTypes.FindIndex(t => t.Id == _editType.Id);
                    if (idx >= 0)
                    {
                        _entityTypes[idx] = _editType;                    
                    }
                }
                else
                {
                    var newEntityType = await Service.AddEntityTypeAsync(_editType, _currentUser?.Name ?? string.Empty);
                    _entityTypes.Add(newEntityType);                
                }
                _showForm = false;
            }
            catch (Exception ex)
            {
                _errorMessage = $"Error saving entity type: {ex.Message}";
            }
            StateHasChanged();
        }
    }
}
