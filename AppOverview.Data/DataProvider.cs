using AppOverview.Model;

namespace AppOverview.Data
{
    public class DataProvider : IDataProvider
    {
        private static List<TechnologyDTO> _technologies = new List<TechnologyDTO>()
        {
            new TechnologyDTO { Id = 1, Name = ".NET", Description = ".NET Framework", IsActive = true },
            new TechnologyDTO { Id = 2, Name = "JavaScript", Description = "JS Language", IsActive = true },
            new TechnologyDTO { Id = 3, Name = "Python", Description = "Python Language", IsActive = false }
        };
        private static List<DepartmentDTO> _departments = new List<DepartmentDTO>
        {
            new DepartmentDTO { Id = 1, Name = "HR", IsActive = true },
            new DepartmentDTO { Id = 2, Name = "IT", IsActive = true },
            new DepartmentDTO { Id = 3, Name = "Finance", IsActive = false }
        };
        private static List<EntityDTO> _entities = new List<EntityDTO>()
        {            
            new EntityDTO { Id = 1, Name = "Entity Alpha", Description = "First test entity", Owner = "John Doe", SourceControlUrl = "www.123.com", TypeId = 1, Type = "Type 1", DepartmentId = 1, Department = "HR", TechnologyId = 1, Technology = ".NET", ColorHex = "#FF0000", IsActive = true, Dependencies = new List<EntityDTO>() },
            new EntityDTO { Id = 2, Name = "Entity Beta", Description = "Second test entity", Owner = "Jane Doe", SourceControlUrl = string.Empty, TypeId = 2, Type = "Type 2", DepartmentId = 2, Department = "IT", TechnologyId = 2, Technology = "JavaScript", ColorHex = "#00FF00", IsActive = true, Dependencies = new List<EntityDTO>() },
            new EntityDTO { Id = 3, Name = "Entity Gamma", Description = "Third test entity", Owner = "Jane Doe", SourceControlUrl = "https://github.com/clemensklemens/AppOverview/tree/main", TypeId = 3, Type = "Type 3", DepartmentId = 3, Department = "Finance", TechnologyId = 3, Technology = "Python", ColorHex = "#0000FF", IsActive = false, Dependencies = new List<EntityDTO>() }
        };
        private static List<EntityTypeDTO> _entityTypes = new List<EntityTypeDTO>()
        {
            new EntityTypeDTO { Id = 1, Name = "Type 1", Description = "Type 1 Desc", ColorHex = "#FF0000", IsActive = true },
            new EntityTypeDTO { Id = 2, Name = "Type 2", Description = "Type 2 Desc", ColorHex = "#00FF00", IsActive = true },
            new EntityTypeDTO { Id = 3, Name = "Type 3", Description = "Type 3 Desc", ColorHex = "#0000FF", IsActive = false }
        };

        public async Task<DepartmentDTO> AddDepartmentAsync(DepartmentDTO department)
        {
            int maxId = _departments.Any() ? _departments.Max(d => d.Id) : 0;
            department.Id = maxId + 1; // Assign a new ID
            _departments.Add(department);
            return department;
        }

        public async Task<EntityDTO> AddEntityAsync(EntityDTO entity)
        {
            int maxId = _entities.Any() ? _entities.Max(e => e.Id) : 0;
            entity.Id = maxId + 1; // Assign a new ID            
            _entities.Add(entity);
            return entity;
        }

        public async Task<EntityTypeDTO> AddEntityTypeAsync(EntityTypeDTO entityType)
        {
            int maxId = _entityTypes.Any() ? _entityTypes.Max(et => et.Id) : 0;
            entityType.Id = maxId + 1; // Assign a new ID
            _entityTypes.Add(entityType);
            return entityType;
        }

        public async Task<TechnologyDTO> AddTechnologyAsync(TechnologyDTO technology)
        {
            int maxId = _technologies.Any() ? _technologies.Max(t => t.Id) : 0;
            technology.Id = maxId + 1; // Assign a new ID
            _technologies.Add(technology);
            return technology;
        }

        public async Task<IEnumerable<DepartmentDTO>> GetDepartmentsAsync(bool activeOnly)
        {
            if(activeOnly)
            {
                return _departments.Where(d => d.IsActive);
            }
            else
            {
                return _departments;
            }                
        }

        public async Task<IEnumerable<IdNameDTO>> GetDepartmentsIdNameListAsync(bool activeOnly)
        {
            if (activeOnly)
            {
                return _departments.Where(d => d.IsActive).Select(d => new IdNameDTO(d.Id, d.Name));
            }
            else
            {                
                return _departments.Select(d => new IdNameDTO(d.Id, d.Name));
            }
        }

        public async Task<IEnumerable<EntityDTO>> GetEntitiesAsync(bool activeOnly)
        {
            if (activeOnly)
            {
                return _entities.Where(d => d.IsActive);
            }
            else
            {
                return _entities;
            }
        }

        public async Task<IEnumerable<IdNameDTO>> GetEntitiesIdNameListAsync(bool activeOnly)
        {
            if (activeOnly)
            {
                return _entities.Where(d => d.IsActive).Select(d => new IdNameDTO(d.Id, d.Name));
            }
            else
            {
                return _entities.Select(d => new IdNameDTO(d.Id, d.Name));
            }
        }

        public async Task<EntityDTO> GetEntityAsync(int id)
        {
            var entity = _entities.FirstOrDefault(e => e.Id == id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"Entity with ID {id} not found.");
            }
            return entity;
        }

        public async Task<IEnumerable<EntityTypeDTO>> GetEntityTypesAsync(bool activeOnly)
        {
            if (activeOnly)
            {
                return _entityTypes.Where(d => d.IsActive);
            }
            else
            {
                return _entityTypes;
            }
        }

        public async Task<IEnumerable<IdNameDTO>> GetEntityTypeIdNameListAsync(bool activeOnly)
        {
            if (activeOnly)
            {
                return _entityTypes.Where(d => d.IsActive).Select(d => new IdNameDTO(d.Id, d.Name));
            }
            else
            {
                return _entityTypes.Select(d => new IdNameDTO(d.Id, d.Name));
            }
        }

        public async Task<IEnumerable<TechnologyDTO>> GetTechnologiesAsync(bool activeOnly)
        {
            if (activeOnly)
            {
                return _technologies.Where(d => d.IsActive);
            }
            else
            {
                return _technologies;
            }
        }

        public async Task<IEnumerable<IdNameDTO>> GetTechnologiesIdNameListAsync(bool activeOnly)
        {
            if (activeOnly)
            {
                return _technologies.Where(d => d.IsActive).Select(d => new IdNameDTO(d.Id, d.Name));
            }
            else
            {
                return _technologies.Select(d => new IdNameDTO(d.Id, d.Name));
            }
        }

        public async Task UpdateDepartmentAsync(DepartmentDTO department)
        {
            var existingDepartment = _departments.FirstOrDefault(d => d.Id == department.Id);
            if (existingDepartment != null)
            {
                existingDepartment.Name = department.Name;
                existingDepartment.IsActive = department.IsActive;
            }
            else
            {
                throw new KeyNotFoundException($"Department with ID {department.Id} not found.");
            }
        }

        public async Task UpdateEntityAsync(EntityDTO entity)
        {
            var existingEntity = _entities.FirstOrDefault(e => e.Id == entity.Id);
            if (existingEntity != null)
            {
                existingEntity.Name = entity.Name;
                existingEntity.Description = entity.Description;
                existingEntity.SourceControlUrl = entity.SourceControlUrl;
                existingEntity.Owner = entity.Owner;
                existingEntity.TypeId = entity.TypeId;
                existingEntity.Type = entity.Type;
                existingEntity.DepartmentId = entity.DepartmentId;
                existingEntity.Department = entity.Department;
                existingEntity.TechnologyId = entity.TechnologyId;
                existingEntity.Technology = entity.Technology;
                existingEntity.ColorHex = entity.ColorHex;
                existingEntity.IsActive = entity.IsActive;
                existingEntity.Dependencies = entity.Dependencies;
            }
            else
            {
                throw new KeyNotFoundException($"Entity with ID {entity.Id} not found.");
            }
        }

        public async Task UpdateEntityTypeAsync(EntityTypeDTO entityType)
        {
            var existingEntityType = _entityTypes.FirstOrDefault(et => et.Id == entityType.Id);
            if (existingEntityType != null)
            {
                existingEntityType.Name = entityType.Name;
                existingEntityType.Description = entityType.Description;
                existingEntityType.ColorHex = entityType.ColorHex;
                existingEntityType.IsActive = entityType.IsActive;
            }
            else
            {
                throw new KeyNotFoundException($"Entity Type with ID {entityType.Id} not found.");
            }
        }

        public async Task UpdateTechnologyAsync(TechnologyDTO technology)
        {
            var existingTechnology = _technologies.FirstOrDefault(t => t.Id == technology.Id);
            if (existingTechnology != null)
            {
                existingTechnology.Name = technology.Name;
                existingTechnology.Description = technology.Description;
                existingTechnology.IsActive = technology.IsActive;
            }
            else
            {
                throw new KeyNotFoundException($"Technology with ID {technology.Id} not found.");
            }
        }
    }
}
