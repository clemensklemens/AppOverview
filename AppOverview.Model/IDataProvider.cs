namespace AppOverview.Model
{
    public interface IDataProvider
    {
        Task<IEnumerable<TechnologyDTO>> GetTechnologiesAsync(bool activeOnly);
        Task<IEnumerable<DepartmentDTO>> GetDepartmentsAsync(bool activeOnly);        
        Task<IEnumerable<EntityTypeDTO>> GetEntityTypesAsync(bool activeOnly);
        Task<IEnumerable<EntityDTO>> GetEntitiesAsync(bool activeOnly);
        Task<EntityDTO> GetEntityAsync(int id);
        Task UpdateTechnologyAsync(TechnologyDTO technology);
        Task UpdateDepartmentAsync(DepartmentDTO department);
        Task UpdateEntityAsync(EntityDTO entity);
        Task UpdateEntityTypeAsync(EntityTypeDTO entityType);
        Task<TechnologyDTO> AddTechnologyAsync(TechnologyDTO technology);
        Task<DepartmentDTO> AddDepartmentAsync(DepartmentDTO department);
        Task<EntityDTO> AddEntityAsync(EntityDTO entity);
        Task<EntityTypeDTO> AddEntityTypeAsync(EntityTypeDTO entityType);
    }
}
