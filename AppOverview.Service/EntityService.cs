using AppOverview.Model;

namespace AppOverview.Service
{
    public class EntityService : IEntityService
    {
        private readonly IDataProvider _dataProvider;

        public EntityService(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public IReadOnlyList<DepartmentDTO> Departments => throw new NotImplementedException();

        public IReadOnlyList<TechnologyDTO> Technologies => throw new NotImplementedException();

        public IReadOnlyList<EntityTypeDTO> EntityTypes => throw new NotImplementedException();

        public async Task<EntityDTO> AddEntityAsync(EntityDTO entity)
        {
            throw new NotImplementedException();
        }

        public async Task<EntityDTO> AddRelatedEntityAsync(int entityId, int relatedEntityId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<EntityDTO>> GetAllEntitiesAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<EntityDTO> GetEntityAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task GetReferenceDataAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<EntityDTO> RemoveRelatedEntityAsync(int entityId, int relatedEntityId)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateEntityAsync(EntityDTO entity)
        {
            throw new NotImplementedException();
        }        
    }
}
