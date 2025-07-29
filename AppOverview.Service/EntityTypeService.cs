using AppOverview.Model;
using Microsoft.Extensions.Logging;

namespace AppOverview.Service
{
    public class EntityTypeService(IDataProvider dataProvider, ILogger<EntityTypeService> logger) : IEntityTypeService
    {
        private readonly IDataProvider _dataProvider = dataProvider;
        private readonly ILogger<EntityTypeService> _logger = logger;

        public async Task<EntityTypeDTO> AddEntityTypeAsync(EntityTypeDTO entityType)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(entityType.Name))
                {
                    throw new ArgumentException("Entity type name cannot be null or empty.", nameof(entityType));
                }
                return await _dataProvider.AddEntityTypeAsync(entityType);
            }
            catch (Exception ex)
            {
                string errorMessage = $"An error occurred while adding the entity type {entityType.Name}.";
                _logger.LogError("{ErrorMessage} Exception: {Exception}", errorMessage, ex);
                throw new ServiceException(errorMessage, ex);
            }
        }

        public async Task<IEnumerable<EntityTypeDTO>> GetEntityTypesAsync()
        {
            try
            {
                return (await _dataProvider.GetEntityTypesAsync());
            }
            catch(Exception ex)
            {
                string errorMessage = "An error occurred while retrieving entity types.";
                _logger.LogError("{ErrorMessage} Exception: {Exception}", errorMessage, ex);
                throw new ServiceException(errorMessage, ex);
            }
        }

        public async Task UpdateEntityTypeAsync(EntityTypeDTO entityType)
        {
            try
            {                
                if (string.IsNullOrWhiteSpace(entityType.Name))
                {
                    throw new ArgumentException("Entity type name cannot be null or empty.", nameof(entityType));
                }
                await _dataProvider.UpdateEntityTypeAsync(entityType);
            }
            catch (Exception ex)
            {
                string errorMessage = $"An error occurred while updating the entity type {entityType.Name}.";
                _logger.LogError("{ErrorMessage} Exception: {Exception}", errorMessage, ex);
                throw new ServiceException(errorMessage, ex);
            }
        }
    }
}
