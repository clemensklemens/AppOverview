using AppOverview.Model.DTOs;
using AppOverview.Model.GraphModel;

namespace AppOverview.Model.Interfaces
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
