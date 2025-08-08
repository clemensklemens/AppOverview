using AppOverview.Model.DTOs;

namespace AppOverview.Model.Interfaces
{
    public interface IDataProvider
    {
        Task<IEnumerable<IdNameDTO>> GetTechnologiesAsync(bool activeOnly);
        Task<IEnumerable<IdNameDTO>> GetDepartmentsAsync(bool activeOnly);        
        Task<IEnumerable<EntityTypeDTO>> GetEntityTypesAsync(bool activeOnly);
        Task<IEnumerable<EntityDTO>> GetEntitiesAsync(bool activeOnly);
        Task<EntityDTO> GetEntityAsync(int id);
        Task UpdateTechnologyAsync(IdNameDTO technology, string userName);
        Task UpdateDepartmentAsync(IdNameDTO department, string userName);
        Task UpdateEntityAsync(EntityDTO entity, string userName);
        Task UpdateEntityTypeAsync(EntityTypeDTO entityType, string userName);
        Task<IdNameDTO> AddTechnologyAsync(IdNameDTO technology, string userName);
        Task<IdNameDTO> AddDepartmentAsync(IdNameDTO department, string userName);
        Task<EntityDTO> AddEntityAsync(EntityDTO entity, string userName);
        Task<EntityTypeDTO> AddEntityTypeAsync(EntityTypeDTO entityType, string userName);
        Task<IEnumerable<IdNameDTO>> GetEntitiesIdNameAsync(bool activeOnly);
        Task<IEnumerable<IdNameDTO>> GetEntityTypeIdNameAsync(bool activeOnly);
    }
}