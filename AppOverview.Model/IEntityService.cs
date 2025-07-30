namespace AppOverview.Model
{
    public interface IEntityService
    {
        Task<IEnumerable<EntityDTO>> GetEntitiesAsync();
        Task<IEnumerable<IdNameDTO>> GetDepartmentsAsync();
        Task<IEnumerable<IdNameDTO>> GetTechnologiesAsync();
        Task<IEnumerable<IdNameDTO>> GetEntityTypesAsync();
        Task<EntityDTO> AddEntityAsync(EntityDTO entity);
        Task UpdateEntityAsync(EntityDTO entity);
        Task<EntityDTO> AddRelatedEntityAsync(int entityId, int relatedEntityId);
        Task<EntityDTO> RemoveRelatedEntityAsync(int entityId, int relatedEntityId);
    }
}
