using AppOverview.Model.DTOs;

namespace AppOverview.Model.Interfaces
{
    public interface IEntityService
    {
        Task<IEnumerable<EntityDTO>> GetEntitiesAsync();
        Task<IEnumerable<IdNameDTO>> GetDepartmentsAsync();
        Task<IEnumerable<IdNameDTO>> GetTechnologiesAsync();
        Task<IEnumerable<IdNameDTO>> GetEntityTypesAsync();
        Task<EntityDTO> AddEntityAsync(EntityDTO entity, string userName);
        Task UpdateEntityAsync(EntityDTO entity, string userName);
        Task<EntityDTO> AddRelatedEntityAsync(int entityId, int relatedEntityId, string userName);
        Task<EntityDTO> RemoveRelatedEntityAsync(int entityId, int relatedEntityId, string userName);
    }
}
