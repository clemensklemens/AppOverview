using AppOverview.Model;

namespace AppOverview.Service
{
    public class EntityTypeService : IEntityTypeService
    {
        private readonly IDataProvider _dataProvider;

        public EntityTypeService(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public async Task<EntityTypeDTO> AddEntityTypeAsync(EntityTypeDTO entityType)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<EntityTypeDTO>> GetAllEntityTypesAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<EntityTypeDTO> GetEntityTypeAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateEntityTypeAsync(EntityTypeDTO entityType)
        {
            throw new NotImplementedException();
        }
    }
}
