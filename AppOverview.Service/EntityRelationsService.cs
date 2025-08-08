using AppOverview.Model;
using AppOverview.Model.DTOs;
using AppOverview.Model.GraphModel;
using AppOverview.Model.Interfaces;
using Microsoft.Extensions.Logging;

namespace AppOverview.Service
{
    public class EntityRelationsService(IDataProvider dataProvider, ILogger<EntityRelationsService> logger) : IEntityRelationsService
    {
        private readonly IDataProvider _dataProvider = dataProvider;
        private readonly ILogger<EntityRelationsService> _logger = logger;
        private List<EntityDTO> _entities = new List<EntityDTO>();
        private List<EntityDTO> _filteredEntities = new List<EntityDTO>();
        private List<IdNameDTO> _types = new List<IdNameDTO>();
        private List<IdNameDTO> _departments = new List<IdNameDTO>();

        public IEnumerable<IdNameDTO> Departments => _departments;
        public IEnumerable<IdNameDTO> EntityTypes => _types;

        public async Task InitServiceAsync()
        {
            try
            {
                _entities = (await _dataProvider.GetEntitiesAsync(true)).ToList();
                _types = _entities.Select(x => new IdNameDTO(){ Id = x.TypeId, Name = x.Type, IsActive = x.IsActive })
                                  .Distinct()
                                  .ToList();

                _departments = _entities.Select(x => new IdNameDTO() { Id = x.TypeId, Name = x.Type, IsActive = x.IsActive })
                                        .Distinct()
                                        .ToList();
            }
            catch (Exception ex)
            {
                string errorMessage = "An error occurred while Service Initialisation.";
                _logger.LogError("{ErrorMessage} Exception: {Exception}", errorMessage, ex);
                throw new ServiceException(errorMessage);
            }
        }

        public void FilterEntities(string? filterText, HashSet<int> selectedTypeIds, HashSet<int> selectedDepartmentIds)
        {
            try
            {
                if (_entities.Count == 0)
                {
                    _filteredEntities = new List<EntityDTO>();
                    return;
                }
                IEnumerable<EntityDTO> filtered = _entities ?? Enumerable.Empty<EntityDTO>();
                
                bool isNameFilter = !string.IsNullOrWhiteSpace(filterText);
                bool isTypeOrDeptFilter = selectedTypeIds.Count > 0 || selectedDepartmentIds.Count > 0;

                if (isNameFilter)
                {
                    filtered = filtered.Where(e => e.Name.Contains(filterText??string.Empty, StringComparison.OrdinalIgnoreCase));
                }
                if (selectedTypeIds.Count > 0)
                {
                    filtered = filtered.Where(e => selectedTypeIds.Contains(e.TypeId));
                }
                if (selectedDepartmentIds.Count > 0)
                {
                    filtered = filtered.Where(e => selectedDepartmentIds.Contains(e.DepartmentId));
                }

                var directMatches = filtered.ToList();
                if (isNameFilter && !isTypeOrDeptFilter)
                {
                    var related = new List<EntityDTO>();
                    foreach (var entity in directMatches)
                    {
                        if (entity.Dependencies is not null)
                        {
                            foreach (var dep in entity.Dependencies)
                            {
                                var depEntity = _entities?.FirstOrDefault(e => e.Id == dep.Id);
                                if (depEntity is not null && !directMatches.Any(m => m.Id == depEntity.Id) && !related.Any(r => r.Id == depEntity.Id))
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
            catch(Exception ex)
            {
                string errorMessage = "An error occurred while filtering entities.";
                _logger.LogError("{ErrorMessage} Exception: {Exception}", errorMessage, ex);
                throw new ServiceException(errorMessage);
            }
        }

        public (IEnumerable<Nodes> nodes, IEnumerable<Edges> edges) GetGraphItems()
        {
            var nodes = new List<Nodes>();
            var edges = new List<Edges>();
            try
            {
                if (_filteredEntities.Count == 0)
                {
                    return (nodes, edges);
                }
                nodes = _filteredEntities.Select(x => new Nodes(
                        id: x.Id,
                        label: x.Name,
                        type: x.Type,
                        description: x.Description,
                        department: x.Department,
                        color: x.ColorHex,
                        owner: x.Owner,
                        url: x.SourceControlUrl
                    )).ToList();

                edges = new List<Edges>();
                foreach (var entity in _filteredEntities)
                {
                    if (entity.Dependencies is not null)
                    {
                        foreach (var dep in entity.Dependencies)
                        {
                            // Only show edges to dependencies that are also in the filtered list
                            if (_filteredEntities.Any(f => f.Id == dep.Id))
                            {
                                edges.Add(new Edges(from: entity.Id, to: dep.Id));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string errorMessage = "An error occurred preparing the graph";
                _logger.LogError("{ErrorMessage} Exception: {Exception}", errorMessage, ex);
                throw new ServiceException(errorMessage);
            }
            return (nodes, edges);
        }
    }
}
