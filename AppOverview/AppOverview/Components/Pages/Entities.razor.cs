// Code-behind for Entities.razor
using AppOverview.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace AppOverview.Components.Pages
{
    public partial class Entities : ComponentBase
    {
        private List<EntityDTO>? _entities;
        private EntityDTO _editEntity = new EntityDTO();
        private bool _showForm = false;
        private bool _isEdit = false;

        protected override async Task OnInitializedAsync()
        {
            var enityData = await Service.GetAllEntitiesAsync();
            _entities = enityData.ToList();
        }

        private void ShowNewForm()
        {
            _editEntity = new EntityDTO();
            _showForm = true;
            _isEdit = false;
        }

        private void ShowEditForm(EntityDTO entity)
        {
            _editEntity = entity;
            _showForm = true;
            _isEdit = true;
        }

        private void CancelEdit()
        {
            _showForm = false;
        }

        private async Task OnSubmitEntityAsync()
        {
            if (_entities == null)
            {
                return;
            }

            if (_isEdit)
            {
                await Service.UpdateEntityAsync(_editEntity);
                var idx = _entities.FindIndex(e => e.Id == _editEntity.Id);
                if (idx >= 0)
                {
                    _entities[idx] = _editEntity;
                }
            }
            else
            {
                var newEntity = await Service.AddEntityAsync(_editEntity);
                _entities.Add(newEntity);
            }
            _showForm = false;
            StateHasChanged();
        }

        private EntityDTO FindNestedValues(EntityDTO dto)
        {
            dto.Department = _departments?.FirstOrDefault(d => d.Id == dto.DepartmentId)?.Name ?? string.Empty;
            dto.Technology = _technologies?.FirstOrDefault(t => t.Id == dto.TechnologyId)?.Name ?? string.Empty;
            dto.Type = _entityTypes?.FirstOrDefault(et => et.Id == dto.TypeId)?.Name ?? string.Empty;
            dto.ColorHex = _entityTypes?.FirstOrDefault(et => et.Id == dto.TypeId)?.ColorHex ?? "#FFFFFF"; // Default color if not found
            return dto;
        }
    }
}
