namespace AppOverview.Model
{
    public interface IEntityTypeService
    {
        Task<EntityTypeDTO> AddEntityTypeAsync(EntityTypeDTO entityType);
        Task<EntityTypeDTO> GetEntityTypeAsync(int id);
        Task<IEnumerable<EntityTypeDTO>> GetAllEntityTypesAsync();
        Task UpdateEntityTypeAsync(EntityTypeDTO entityType);
    }
}
