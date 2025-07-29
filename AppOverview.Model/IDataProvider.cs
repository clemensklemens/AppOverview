namespace AppOverview.Model
{
    public interface IDataProvider
    {
        Task<IEnumerable<TechnologyDTO>> GetTechnologiesAsync();
        Task<IEnumerable<DepartmentDTO>> GetDepartmentsAsync();        
        Task<IEnumerable<EntityTypeDTO>> GetEntityTypesAsync();
        Task<IEnumerable<EntityDTO>> GetEntitiesAsync();
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
