using AppOverview.Model;
using AppOverview.Model.DTOs;
using AppOverview.Model.Interfaces;
using Microsoft.Extensions.Logging;

namespace AppOverview.Service
{
    public class EntityTypeService(IDataProvider dataProvider, ILogger<EntityTypeService> logger) : IEntityTypeService
    {
        private readonly IDataProvider _dataProvider = dataProvider;
        private readonly ILogger<EntityTypeService> _logger = logger;

        public async Task<EntityTypeDTO> AddEntityTypeAsync(EntityTypeDTO entityType, string userName)
        {
            if (string.IsNullOrWhiteSpace(entityType.Name))
            {
                string errorMessage = "Entity type name cannot be null or empty.";
                var ex = new ArgumentException(errorMessage, nameof(entityType));
                _logger.LogError("{ErrorMessage} Technology: {Technology}", errorMessage, ex);
                throw ex;
            }

            try
            {
                return await _dataProvider.AddEntityTypeAsync(entityType, userName);
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
                return (await _dataProvider.GetEntityTypesAsync(false));
            }
            catch(Exception ex)
            {
                string errorMessage = "An error occurred while retrieving entity types.";
                _logger.LogError("{ErrorMessage} Exception: {Exception}", errorMessage, ex);
                throw new ServiceException(errorMessage, ex);
            }
        }

        public async Task UpdateEntityTypeAsync(EntityTypeDTO entityType, string userName)
        {
            if (string.IsNullOrWhiteSpace(entityType.Name))
            {
                string errorMessage = "Entity type name cannot be null or empty.";
                var ex = new ArgumentException(errorMessage, nameof(entityType));
                _logger.LogError("{ErrorMessage} Technology: {Technology}", errorMessage, ex);
                throw ex;
            }

            try
            {                
                await _dataProvider.UpdateEntityTypeAsync(entityType, userName);
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
