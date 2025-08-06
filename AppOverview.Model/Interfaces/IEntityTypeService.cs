using AppOverview.Model.DTOs;

namespace AppOverview.Model.Interfaces
{
    public interface IEntityTypeService
    {
        Task<EntityTypeDTO> AddEntityTypeAsync(EntityTypeDTO entityType);
        Task<IEnumerable<EntityTypeDTO>> GetEntityTypesAsync();
        Task UpdateEntityTypeAsync(EntityTypeDTO entityType);
    }
}
