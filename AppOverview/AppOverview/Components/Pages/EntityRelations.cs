using AppOverview.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace AppOverview.Components.Pages
{
    public partial class EntityRelations : ComponentBase
    {
        private List<EntityDTO>? _entities;
        private List<EntityDTO>? _filteredEntities;
        private List<IdNameDTO>? _allTypes;
        private List<DepartmentDTO>? _allDepartments;
        private HashSet<int> _selectedTypeIds = new();
        private HashSet<int> _selectedDepartmentIds = new();
        private string _filterText = string.Empty;
        private string FilterText
        {
            get => _filterText;
            set
            {
                if (_filterText != value)
                {
                    _filterText = value;
                    FilterEntities();
                    StateHasChanged(); // This will trigger OnAfterRenderAsync
                }
            }
        }
        private HashSet<int> _expanded = new();
        private bool _typeFiltersExpanded = true;
        private bool _departmentFiltersExpanded = true;

        protected override async Task OnInitializedAsync()
        {
            _entities = (await Service.GetEntitiesAsync()).Where(x => x.IsActive).OrderBy(x => x.Name).ToList();
            _allTypes = (await EntityTypeService.GetEntityTypesAsync()).Select(t => new IdNameDTO(t.Id, t.Name)).ToList();
            _allDepartments = (await DepartmentService.GetDepartmentsAsync()).ToList();
            FilterEntities();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (_entities != null)
            {
                await RenderGraphAsync();
            }
        }

        private void ToggleTypeFilters()
        {
            _typeFiltersExpanded = !_typeFiltersExpanded;
        }
        private void ToggleDepartmentFilters()
        {
            _departmentFiltersExpanded = !_departmentFiltersExpanded;
        }

        private void OnTypeChecked(int id, object? value)
        {
            if (value is bool isChecked && isChecked)
                _selectedTypeIds.Add(id);
            else
                _selectedTypeIds.Remove(id);
            FilterEntities();
            StateHasChanged();
        }

        private void OnDepartmentChecked(int id, object? value)
        {
            if (value is bool isChecked && isChecked)
                _selectedDepartmentIds.Add(id);
            else
                _selectedDepartmentIds.Remove(id);
            FilterEntities();
            StateHasChanged();
        }

        private void FilterEntities()
        {
            IEnumerable<EntityDTO> filtered = _entities ?? Enumerable.Empty<EntityDTO>();

            bool isNameFilter = !string.IsNullOrWhiteSpace(_filterText);
            bool isTypeOrDeptFilter = _selectedTypeIds.Any() || _selectedDepartmentIds.Any();

            if (isNameFilter)
            {
                filtered = filtered.Where(e => e.Name.Contains(_filterText, StringComparison.OrdinalIgnoreCase));
            }
            if (_selectedTypeIds.Any())
            {
                filtered = filtered.Where(e => _selectedTypeIds.Contains(e.TypeId));
            }
            if (_selectedDepartmentIds.Any())
            {
                filtered = filtered.Where(e => _selectedDepartmentIds.Contains(e.DepartmentId));
            }

            var directMatches = filtered.ToList();
            if (isNameFilter && !isTypeOrDeptFilter)
            {
                var related = new List<EntityDTO>();
                foreach (var entity in directMatches)
                {
                    if (entity.Dependencies != null)
                    {
                        foreach (var dep in entity.Dependencies)
                        {
                            var depEntity = _entities.FirstOrDefault(e => e.Id == dep.Id);
                            if (depEntity != null && !directMatches.Any(m => m.Id == depEntity.Id) && !related.Any(r => r.Id == depEntity.Id))
                            {
                                related.Add(depEntity);
                            }
                        }
                    }
                }
                _filteredEntities = directMatches.Concat(related).ToList();
            }
            else
            {
                _filteredEntities = directMatches;
            }
        }

        private void ToggleExpand(int entityId)
        {
            if (_expanded.Contains(entityId))
                _expanded.Remove(entityId);
            else
                _expanded.Add(entityId);
        }

        private async Task RenderGraphAsync()
        {
            if (_filteredEntities == null) return;

            var nodes = _filteredEntities
                .Select(e => new
                {
                    id = e.Id,
                    label = e.Name,
                    department = e.Department,
                    type = e.Type,
                    description = e.Description,
                    color = e.ColorHex,
                    owner = e.Owner,
                    url = e.SourceControlUrl
                })
                .ToList();

            var edges = new List<object>();
            foreach (var entity in _filteredEntities)
            {
                if (entity.Dependencies != null)
                {
                    foreach (var dep in entity.Dependencies)
                    {
                        // Only show edges to dependencies that are also in the filtered list
                        if (_filteredEntities.Any(f => f.Id == dep.Id))
                        {
                            edges.Add(new { from = entity.Id, to = dep.Id });
                        }
                    }
                }
            }

            await JS.InvokeVoidAsync("entityGraph.render", "entity-graph", nodes, edges);
        }
    }
}