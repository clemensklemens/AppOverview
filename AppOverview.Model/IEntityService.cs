namespace AppOverview.Model
{
    public interface IEntityService
    {
        Task<IEnumerable<EntityDTO>> GetEntitiesAsync();
        Task<IEnumerable<DepartmentDTO>> GetDepartmentsAsync();
        Task<IEnumerable<TechnologyDTO>> GetTechnologiesAsync();
        Task<IEnumerable<EntityTypeDTO>> GetEntityTypesAsync();
        Task<EntityDTO> AddEntityAsync(EntityDTO entity);
        Task UpdateEntityAsync(EntityDTO entity);
        Task<EntityDTO> AddRelatedEntityAsync(int entityId, int relatedEntityId);
        Task<EntityDTO> RemoveRelatedEntityAsync(int entityId, int relatedEntityId);
    }
}
