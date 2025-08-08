using AppOverview.Model.DTOs;

namespace AppOverview.Model.Interfaces
{
    public interface IDataProvider
    {
        Task<IEnumerable<TechnologyDTO>> GetTechnologiesAsync(bool activeOnly);
        Task<IEnumerable<DepartmentDTO>> GetDepartmentsAsync(bool activeOnly);        
        Task<IEnumerable<EntityTypeDTO>> GetEntityTypesAsync(bool activeOnly);
        Task<IEnumerable<EntityDTO>> GetEntitiesAsync(bool activeOnly);
        Task<EntityDTO> GetEntityAsync(int id);
        Task UpdateTechnologyAsync(TechnologyDTO technology, string userName);
        Task UpdateDepartmentAsync(DepartmentDTO department, string userName);
        Task UpdateEntityAsync(EntityDTO entity, string userName);
        Task UpdateEntityTypeAsync(EntityTypeDTO entityType, string userName);
        Task<TechnologyDTO> AddTechnologyAsync(TechnologyDTO technology, string userName);
        Task<DepartmentDTO> AddDepartmentAsync(DepartmentDTO department, string userName);
        Task<EntityDTO> AddEntityAsync(EntityDTO entity, string userName);
        Task<EntityTypeDTO> AddEntityTypeAsync(EntityTypeDTO entityType, string userName);
        Task<IEnumerable<IdNameDTO>> GetEntitiesIdNameListAsync(bool activeOnly);
        Task<IEnumerable<IdNameDTO>> GetEntityTypeIdNameListAsync(bool activeOnly);
        Task<IEnumerable<IdNameDTO>> GetTechnologiesIdNameListAsync(bool activeOnly);
        Task<IEnumerable<IdNameDTO>> GetDepartmentsIdNameListAsync(bool activeOnly);
    }
}