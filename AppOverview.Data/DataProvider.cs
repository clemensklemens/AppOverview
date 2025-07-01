using AppOverview.Model;

namespace AppOverview.Data
{
    public class DataProvider : IDataProvider
    {
        public IEnumerable<DepartmentDTO> GetDepartments()
        {
            return new[]
            {
                new DepartmentDTO(1, "HR", true),
                new DepartmentDTO(2, "IT", true),
                new DepartmentDTO(3, "Finance", false)
            };
        }

        public IEnumerable<EntityDTO> GetEntities()
        {
            return new[]
            {
                new EntityDTO(1, "Entity A", "Description A", "Type 1", "HR", ".NET", "#FF0000", true),
                new EntityDTO(2, "Entity B", "Description B", "Type 2", "IT", "JavaScript", "#00FF00", true),
                new EntityDTO(3, "Entity C", "Description C", "Type 3", "Finance", "Python", "#0000FF", false)
            };
        }

        public IEnumerable<EnityTypeDTO> GetEntityTypes()
        {
            return new[]
            {
                new EnityTypeDTO(1, "Type 1", "Type 1 Desc", "#FF0000", true),
                new EnityTypeDTO(2, "Type 2", "Type 2 Desc", "#00FF00", true),
                new EnityTypeDTO(3, "Type 3", "Type 3 Desc", "#0000FF", false)
            };
        }

        public IEnumerable<TechnologyDTO> GetTechnologies()
        {
            return new[]
            {
                new TechnologyDTO(1, ".NET", ".NET Framework", true),
                new TechnologyDTO(2, "JavaScript", "JS Language", true),
                new TechnologyDTO(3, "Python", "Python Language", false)
            };
        }
    }
}
