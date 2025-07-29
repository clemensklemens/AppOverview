namespace AppOverview.Model
{
    public interface IEntityTypeService
    {
        Task<EntityTypeDTO> AddEntityTypeAsync(EntityTypeDTO entityType);
        Task<IEnumerable<EntityTypeDTO>> GetEntityTypesAsync();
        Task UpdateEntityTypeAsync(EntityTypeDTO entityType);
    }
}
