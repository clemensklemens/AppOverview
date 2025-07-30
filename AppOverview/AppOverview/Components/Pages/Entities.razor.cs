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
        private List<IdNameDTO>? _departments;
        private List<IdNameDTO>? _technologies;
        private List<IdNameDTO>? _entityTypes;

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

        private bool _showDependenciesEditor = false;
        private EntityDTO? _editingDependenciesEntity = null;
        private string _dependencySearchText = string.Empty;
        private List<EntityDTO> _dependencyCandidates = new();

        private void ShowEditDependencies(EntityDTO entity)
        {
            _editingDependenciesEntity = entity;
            _showDependenciesEditor = true;
            _dependencySearchText = string.Empty;
            UpdateDependencyCandidates();
        }

        private void HideEditDependencies()
        {
            _showDependenciesEditor = false;
            _editingDependenciesEntity = null;
            _dependencySearchText = string.Empty;
            _dependencyCandidates.Clear();
        }

        private void UpdateDependencyCandidates()
        {
            if (_entities == null || _editingDependenciesEntity == null)
            {
                _dependencyCandidates = new();
                return;
            }
            var excludeIds = _editingDependenciesEntity.Dependencies.Select(d => d.Id).Append(_editingDependenciesEntity.Id).ToHashSet();
            _dependencyCandidates = _entities
                .Where(e => !excludeIds.Contains(e.Id) && (string.IsNullOrWhiteSpace(_dependencySearchText) || e.Name.Contains(_dependencySearchText, StringComparison.OrdinalIgnoreCase)))
                .ToList();
        }

        private async Task AddDependency(EntityDTO dep)
        {
            if (_editingDependenciesEntity == null)
            {
                return;
            }
            
            await Service.AddRelatedEntityAsync(_editingDependenciesEntity.Id, dep.Id);
            _editingDependenciesEntity.Dependencies.Add(dep);
            UpdateDependencyCandidates();
            StateHasChanged();
        }

        private async Task RemoveDependency(EntityDTO dep)
        {
            if (_editingDependenciesEntity == null) return;
            await Service.RemoveRelatedEntityAsync(_editingDependenciesEntity.Id, dep.Id);
            _editingDependenciesEntity.Dependencies.RemoveAll(d => d.Id == dep.Id);
            UpdateDependencyCandidates();
            StateHasChanged();
        }

        private string DependencySearchText
        {
            get => _dependencySearchText;
            set
            {
                if (_dependencySearchText != value)
                {
                    _dependencySearchText = value;
                    UpdateDependencyCandidates();
                    StateHasChanged();
                }
            }
        }
    }
}