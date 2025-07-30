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

        protected bool _nameInvalid = false;
        protected bool _descriptionInvalid = false;
        protected bool _departmentInvalid = false;
        protected bool _entityTypeInvalid = false;
        protected bool _technologyInvalid = false;

        protected override async Task OnInitializedAsync()
        {
            _entities = (await Service.GetEntitiesAsync()).OrderBy(x => x.Name).ToList();
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

            // Validate required fields
            _nameInvalid = string.IsNullOrWhiteSpace(_editEntity.Name);
            _descriptionInvalid = string.IsNullOrWhiteSpace(_editEntity.Description);
            _departmentInvalid = _editEntity.DepartmentId == 0;
            _entityTypeInvalid = _editEntity.TypeId == 0;
            _technologyInvalid = _editEntity.TechnologyId == 0;

            if (_nameInvalid || _descriptionInvalid || _departmentInvalid || _entityTypeInvalid || _technologyInvalid)
            {
                StateHasChanged();
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