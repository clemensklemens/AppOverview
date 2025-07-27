namespace AppOverview.Model
{
    public interface IEntityService
    {        
        IReadOnlyList<DepartmentDTO> Departments { get; }
        IReadOnlyList<TechnologyDTO> Technologies { get; }
        IReadOnlyList<EntityTypeDTO> EntityTypes { get; }

        Task GetReferenceDataAsync();
        Task<EntityDTO> AddEntityAsync(EntityDTO entity);
        Task<EntityDTO> GetEntityAsync(int id);
        Task<IEnumerable<EntityDTO>> GetAllEntitiesAsync();
        Task UpdateEntityAsync(EntityDTO entity);
        Task<EntityDTO> AddRelatedEntityAsync(int entityId, int relatedEntityId);
        Task<EntityDTO> RemoveRelatedEntityAsync(int entityId, int relatedEntityId);
    }
}
