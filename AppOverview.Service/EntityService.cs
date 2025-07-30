using AppOverview.Model;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace AppOverview.Service
{
    public class EntityService(IDataProvider dataProvider, ILogger<EntityService> logger) : IEntityService
    {
        private readonly IDataProvider _dataProvider = dataProvider;
        private readonly ILogger<EntityService> _logger = logger;

        #region Public Methods

        public async Task<EntityDTO> AddEntityAsync(EntityDTO entity)
        {
            try
            {
                if(!IsValidEntity(entity))
                {
                    string errorMessage = "Invalid entity data provided.";
                    _logger.LogError("{ErrorMessage}", errorMessage);
                    throw new ArgumentException(errorMessage);
                }
                var newEntity = await _dataProvider.AddEntityAsync(entity);
                return newEntity;
            }
            catch (Exception ex)
            {
                string errorMessage = "An error occurred while adding the entity.";
                _logger.LogError("{ErrorMessage} Exception: {Exception}", errorMessage, ex);
                throw new ServiceException(errorMessage);
            }
        }

        public async Task<EntityDTO> AddRelatedEntityAsync(int entityId, int relatedEntityId)
        {
            try
            {
                var entity = await _dataProvider.GetEntityAsync(entityId);
                var relatedEntity = await _dataProvider.GetEntityAsync(relatedEntityId);

                if (!entity.Dependencies.Any(dependency => dependency.Id == relatedEntity.Id))
                {
                    entity.Dependencies.Add(relatedEntity);
                }
                
                await _dataProvider.UpdateEntityAsync(entity);
                return entity;
            }
            catch (Exception ex)
            {
                string errorMessage = "An error occurred while adding an entity relation.";
                _logger.LogError("{ErrorMessage} Exception: {Exception}", errorMessage, ex);
                throw new ServiceException(errorMessage);
            }
        }

        public async Task<EntityDTO> RemoveRelatedEntityAsync(int entityId, int relatedEntityId)
        {
            try
            {
                var entity = await _dataProvider.GetEntityAsync(entityId);
                var relatedEntity = await _dataProvider.GetEntityAsync(relatedEntityId);

                if (entity.Dependencies.Any(dependency => dependency.Id == relatedEntity.Id))
                {
                    entity.Dependencies.Remove(relatedEntity);
                }

                await _dataProvider.UpdateEntityAsync(entity);
                return entity;
            }
            catch (Exception ex)
            {
                string errorMessage = "An error occurred while removing an entity relation.";
                _logger.LogError("{ErrorMessage} Exception: {Exception}", errorMessage, ex);
                throw new ServiceException(errorMessage);
            }
        }

        public async Task UpdateEntityAsync(EntityDTO entity)
        {
            try
            {
                await _dataProvider.UpdateEntityAsync(entity);
            }
            catch (Exception ex)
            {
                string errorMessage = "An error occurred while updating the entity.";
                _logger.LogError("{ErrorMessage} Exception: {Exception}", errorMessage, ex);
                throw new ServiceException(errorMessage);
            }
        }

        public async Task<IEnumerable<EntityDTO>> GetEntitiesAsync()
        {
            try
            {
                return await _dataProvider.GetEntitiesAsync(false);
            }
            catch (Exception ex)
            {
                string errorMessage = "An error occurred while retrieving entities.";
                _logger.LogError("{ErrorMessage} Exception: {Exception}", errorMessage, ex);
                throw new ServiceException(errorMessage);
            }
        }

        public async Task<IEnumerable<DepartmentDTO>> GetDepartmentsAsync()
        {
            try
            {
                return (await _dataProvider.GetDepartmentsAsync(true)).OrderBy(x => x.Name).ToList();
            }
            catch (Exception ex)
            {
                string errorMessage = "An error occurred while retrieving departments.";
                _logger.LogError("{ErrorMessage} Exception: {Exception}", errorMessage, ex);
                throw new ServiceException(errorMessage);
            }
        }

        public async Task<IEnumerable<TechnologyDTO>> GetTechnologiesAsync()
        {
            try
            {
                return (await _dataProvider.GetTechnologiesAsync(true)).OrderBy(x => x.Name).ToList();
            }
            catch (Exception ex)
            {
                string errorMessage = "An error occurred while retrieving technologies.";
                _logger.LogError("{ErrorMessage} Exception: {Exception}", errorMessage, ex);
                throw new ServiceException(errorMessage);
            }
        }

        public async Task<IEnumerable<EntityTypeDTO>> GetEntityTypesAsync()
        {
            try
            {
                return (await _dataProvider.GetEntityTypesAsync(true)).OrderBy(x => x.Name).ToList();
            }
            catch (Exception ex)
            {
                string errorMessage = "An error occurred while retrieving entity types.";
                _logger.LogError("{ErrorMessage} Exception: {Exception}", errorMessage, ex);
                throw new ServiceException(errorMessage);
            }
        }

        #endregion
        #region Private Methods

        private static bool IsValidEntity(EntityDTO entity)
        {
            return 
                entity.TypeId > 0 &&
                entity.DepartmentId > 0 && entity.TechnologyId > 0 &&
                !string.IsNullOrWhiteSpace(entity.Name);
        }
        #endregion
    }
}
