// Code-behind for Entities.razor
using AppOverview.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace AppOverview.Components.Pages
{
    public partial class Entities : ComponentBase
    {
        private List<EntityDTO>? _entities;
        private EntityDTO _editEntity = new();
        private bool _showForm = false;
        private bool _isEdit = false;
        private List<DepartmentDTO>? _departments;
        private List<TechnologyDTO>? _technologies;
        private List<EntityTypeDTO>? _entityTypes;

        protected override async Task OnInitializedAsync()
        {
            _entities = (await Service.GetEntitiesAsync()).OrderBy(x => x.Name).ToList();
            await Service.GetReferenceDataAsync();
            _departments = (await Service.GetDepartmentsAsync()).OrderBy(x => x.Name).ToList();
            _technologies = (await Service.GetTechnologiesAsync()).OrderBy(x => x.Name).ToList();
            _entityTypes = (await Service.GetEntityTypesAsync()).OrderBy(x => x.Name).ToList();
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
    }
}