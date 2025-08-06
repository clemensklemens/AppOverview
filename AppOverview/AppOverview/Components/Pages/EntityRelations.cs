// Code-behind for EntityRelations.razor
using AppOverview.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace AppOverview.Components.Pages
{
    public partial class EntityRelations : ComponentBase
    {
        private List<IdNameDTO>? _allTypes;
        private List<IdNameDTO>? _allDepartments;
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
                    Service.FilterEntities(_filterText, _selectedTypeIds, _selectedDepartmentIds);
                    StateHasChanged(); // This will trigger OnAfterRenderAsync
                }
            }
        }
        private HashSet<int> _expanded = new();
        private bool _typeFiltersExpanded = true;
        private bool _departmentFiltersExpanded = true;

        protected override async Task OnInitializedAsync()
        {
            await Service.InitServiceAsync();
            _allTypes = Service.EntityTypes.OrderBy(x => x.Name).ToList();
            _allDepartments = Service.Departments.OrderBy(x => x.Name).ToList();
            Service.FilterEntities(_filterText, _selectedTypeIds, _selectedDepartmentIds);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await RenderGraphAsync();           
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
            {
                _selectedTypeIds.Add(id);
            }
            else
            {
                _selectedTypeIds.Remove(id);
            }
            Service.FilterEntities(_filterText, _selectedTypeIds, _selectedDepartmentIds);
            StateHasChanged();
        }

        private void OnDepartmentChecked(int id, object? value)
        {
            if (value is bool isChecked && isChecked)
            {
                _selectedDepartmentIds.Add(id);
            }
            else
            {
                _selectedDepartmentIds.Remove(id);
            }
            Service.FilterEntities(_filterText, _selectedTypeIds, _selectedDepartmentIds);
            StateHasChanged();
        }        

        private async Task RenderGraphAsync()
        {
            (var nodes, var edges) = Service.GetGraphItems();
            await JS.InvokeVoidAsync("entityGraph.render", "entity-graph", nodes, edges);
        }
    }
}