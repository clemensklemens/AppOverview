namespace AppOverview.Model
{
    public interface IEntityRelationsService
    {
        Task InitServiceAsync();
        void FilterEntities(string? filterText, HashSet<int> selectedTypeIds, HashSet<int> selectedDepartmentIds);
        (IEnumerable<Nodes> nodes, IEnumerable<Edges> edges) GetGraphItems();
        public IEnumerable<IdNameDTO> Departments { get; }
        public IEnumerable<IdNameDTO> EntityTypes { get; }
    }
}
