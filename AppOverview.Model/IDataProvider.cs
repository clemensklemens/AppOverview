namespace AppOverview.Model
{
    public interface IDataProvider
    {
        Task<IEnumerable<TechnologyDTO>> GetTechnologiesAsync();
        Task<IEnumerable<DepartmentDTO>> GetDepartmentsAsync();
        Task<IEnumerable<EntityDTO>> GetEntitiesAsync();
        Task<IEnumerable<EntityTypeDTO>> GetEntityTypesAsync();
        Task UpdateTechnologyAsync(TechnologyDTO technology);
        Task UpdateDepartmentAsync(DepartmentDTO department);
        Task UpdateEntityAsync(EntityDTO entity);
        Task UpdateEntityTypeAsync(EntityTypeDTO entityType);
        Task AddTechnologyAsync(TechnologyDTO technology);
        Task AddDepartmentAsync(DepartmentDTO department);
        Task AddEntityAsync(EntityDTO entity);
        Task AddEntityTypeAsync(EntityTypeDTO entityType);
    }
}
