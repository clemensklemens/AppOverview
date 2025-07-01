namespace AppOverview.Model
{
    public interface IDataProvider
    {
        IEnumerable<TechnologyDTO> GetTechnologies();
        IEnumerable<DepartmentDTO> GetDepartments();
        IEnumerable<EntityDTO> GetEntities();
        IEnumerable<EnityTypeDTO> GetEntityTypes();
    }
}
