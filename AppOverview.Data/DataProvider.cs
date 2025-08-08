using AppOverview.Data.Models;
using AppOverview.Model.DTOs;
using AppOverview.Model.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AppOverview.Data
{
    public class DataProvider(string connectionString) : IDataProvider
    {
        private readonly string _connetionString = connectionString;

        public async Task<IEnumerable<IdNameDTO>> GetTechnologiesAsync(bool activeOnly)
        {
            List<IdNameDTO> technologies = new List<IdNameDTO>();
            using var context = new AppOverviewContext(_connetionString);
            var technologiesQry = context.Technologies.Where(x => !activeOnly || x.Active);
            await technologiesQry.ForEachAsync(technology =>
                technologies.Add(new IdNameDTO
                {
                    Id = technology.TechnologyId,
                    Name = technology.Name,
                    IsActive = technology.Active
                }));
            return technologies;
        }

        public async Task<IEnumerable<IdNameDTO>> GetDepartmentsAsync(bool activeOnly)
        {
            List<IdNameDTO> departments = new List<IdNameDTO>();
            using var context = new AppOverviewContext(_connetionString);
            var departmentsQry = context.Departments.Where(x => !activeOnly || x.Active);
            await departmentsQry.ForEachAsync(department =>
                departments.Add(new IdNameDTO
                {
                    Id = department.DepartmentId,
                    Name = department.Name,
                    IsActive = department.Active
                }));
            return departments;
        }

        public async Task<IEnumerable<EntityTypeDTO>> GetEntityTypesAsync(bool activeOnly)
        {
            List<EntityTypeDTO> entityTypes = new List<EntityTypeDTO>();
            using var context = new AppOverviewContext(_connetionString);
            var entityTypesQry = context.EntityTypes.Where(x => !activeOnly || x.Active);
            await entityTypesQry.ForEachAsync(entityType =>
                entityTypes.Add(new EntityTypeDTO
                {
                    Id = entityType.EntityTypeId,
                    Name = entityType.Name,
                    ColorHex = entityType.Apperance,
                    IsActive = entityType.Active
                }));
            return entityTypes;
        }

        public async Task<IEnumerable<EntityDTO>> GetEntitiesAsync(bool activeOnly)
        {
            List<EntityDTO> entities = new List<EntityDTO>();
            using var context = new AppOverviewContext(_connetionString);
            var entitiesQry = context.Entities
                .Include(e => e.EntityType)
                .Include(e => e.Department)
                .Include(e => e.Technology)
                .Where(x => !activeOnly || x.Active);

            await entitiesQry.ForEachAsync(entity =>
            {
                var entityDto = new EntityDTO
                {
                    Id = entity.EntityId,
                    Name = entity.Name,
                    Description = entity.Description ?? string.Empty,
                    TypeId = entity.EntityTypeId,
                    Type = entity.EntityType?.Name ?? string.Empty,
                    DepartmentId = entity.DepartmentId,
                    Department = entity.Department?.Name ?? string.Empty,
                    TechnologyId = entity.TechnologyId,
                    Technology = entity.Technology?.Name ?? string.Empty,
                    SourceControlUrl = entity.SourceControlUrl ?? string.Empty,
                    Owner = entity.Owner ?? string.Empty,
                    ColorHex = entity.EntityType?.Apperance ?? "#000000",
                    IsActive = entity.Active,
                    Dependencies = new List<EntityDTO>()
                };
                entityDto.Dependencies = LoadDependenciesAsync(entity.EntityId).Result.ToList();
                entities.Add(entityDto);
            });
            return entities;
        }

        public async Task<EntityDTO> GetEntityAsync(int id)
        {
            using var context = new AppOverviewContext(_connetionString);
            var entity = await context.Entities.Include(e => e.EntityType)
                .Include(e => e.Department)
                .Include(e => e.Technology)
                .FirstOrDefaultAsync(e => e.EntityId == id);
            if (entity is null)
            {
                throw new KeyNotFoundException($"Entity with ID {id} not found.");
            }

            var entityDto = new EntityDTO
            {
                Id = entity.EntityId,
                Name = entity.Name,
                Description = entity.Description ?? string.Empty,
                TypeId = entity.EntityTypeId,
                Type = entity.EntityType?.Name ?? string.Empty,
                DepartmentId = entity.DepartmentId,
                Department = entity.Department?.Name ?? string.Empty,
                TechnologyId = entity.TechnologyId,
                Technology = entity.Technology?.Name ?? string.Empty,
                SourceControlUrl = entity.SourceControlUrl ?? string.Empty,
                Owner = entity.Owner ?? string.Empty,
                ColorHex = entity.EntityType?.Apperance ?? "#000000",
                IsActive = entity.Active,
                Dependencies = new List<EntityDTO>()
            };
            entityDto.Dependencies = (await LoadDependenciesAsync(entity.EntityId)).ToList();
            return entityDto;
        }

        public async Task UpdateTechnologyAsync(IdNameDTO technologyDto, string userName)
        {
            using var context = new AppOverviewContext(_connetionString);
            var technology = await context.Technologies.FirstOrDefaultAsync(t => t.TechnologyId == technologyDto.Id);
            if (technology is null)
            {
                throw new KeyNotFoundException($"Technology with ID {technologyDto.Id} not found.");
            }

            technology.Name = technologyDto.Name;
            technology.Active = technologyDto.IsActive;
            technology.LastUser = userName;
            technology.LastChange = DateTime.UtcNow;
            await context.SaveChangesAsync();
        }

        public async Task UpdateDepartmentAsync(IdNameDTO departmentDto, string userName)
        {
            using var context = new AppOverviewContext(_connetionString);
            var department = await context.Departments.FirstOrDefaultAsync(d => d.DepartmentId == departmentDto.Id);
            if (department is null)
            {
                throw new KeyNotFoundException($"Department with ID {departmentDto.Id} not found.");
            }

            department.Name = departmentDto.Name;
            department.Active = departmentDto.IsActive;
            department.LastUser = userName;
            department.LastChange = DateTime.UtcNow;
            await context.SaveChangesAsync();
        }

        public async Task UpdateEntityAsync(EntityDTO entityDto, string userName)
        {
            using var context = new AppOverviewContext(_connetionString);
            var entity = await context.Entities.FirstOrDefaultAsync(e => e.EntityId == entityDto.Id);
            if (entity is null)
            {
                throw new KeyNotFoundException($"Entity with ID {entityDto.Id} not found.");
            }

            entity.Name = entityDto.Name;
            entity.Description = entityDto.Description;
            entity.SourceControlUrl = entityDto.SourceControlUrl;
            entity.Owner = entityDto.Owner;
            entity.EntityTypeId = entityDto.TypeId;
            entity.DepartmentId = entityDto.DepartmentId;
            entity.TechnologyId = entityDto.TechnologyId;
            entity.Active = entityDto.IsActive;
            entity.LastUser = userName;
            entity.LastChange = DateTime.UtcNow;
            await context.SaveChangesAsync();
        }

        public async Task UpdateEntityTypeAsync(EntityTypeDTO entityType, string userName)
        {
            using var context = new AppOverviewContext(_connetionString);
            var et = await context.EntityTypes.FirstOrDefaultAsync(e => e.EntityTypeId == entityType.Id);
            if (et is null)
            {
                throw new KeyNotFoundException($"Entity Type with ID {entityType.Id} not found.");
            }

            et.Name = entityType.Name;
            et.Apperance = entityType.ColorHex;
            et.Active = entityType.IsActive;
            et.LastUser = userName;
            et.LastChange = DateTime.UtcNow;
            await context.SaveChangesAsync();
        }

        public async Task<IdNameDTO> AddTechnologyAsync(IdNameDTO technology, string userName)
        {
            using var context = new AppOverviewContext(_connetionString);
            var tech = new Technology
            {
                Name = technology.Name,
                Active = technology.IsActive,
                LastUser = userName,
                LastChange = DateTime.UtcNow
            };
            context.Technologies.Add(tech);
            await context.SaveChangesAsync();
            technology.Id = tech.TechnologyId;
            return technology;
        }

        public async Task<IdNameDTO> AddDepartmentAsync(IdNameDTO department, string userName)
        {
            using var context = new AppOverviewContext(_connetionString);
            var dep = new Department
            {
                Name = department.Name,
                Active = department.IsActive,
                LastUser = userName,
                LastChange = DateTime.UtcNow
            };
            context.Departments.Add(dep);
            await context.SaveChangesAsync();
            department.Id = dep.DepartmentId;
            return department;
        }

        public async Task<EntityDTO> AddEntityAsync(EntityDTO entity, string userName)
        {
            using var context = new AppOverviewContext(_connetionString);
            var ent = new Entity
            {
                Name = entity.Name,
                Description = entity.Description,
                SourceControlUrl = entity.SourceControlUrl,
                Owner = entity.Owner,
                EntityTypeId = entity.TypeId,
                DepartmentId = entity.DepartmentId,
                TechnologyId = entity.TechnologyId,
                Active = entity.IsActive,
                LastUser = userName,
                LastChange = DateTime.UtcNow
            };
            context.Entities.Add(ent);
            await context.SaveChangesAsync();
            entity.Id = ent.EntityId;
            return entity;
        }

        public async Task<EntityTypeDTO> AddEntityTypeAsync(EntityTypeDTO entityType, string userName)
        {
            using var context = new AppOverviewContext(_connetionString);
            var et = new EntityType
            {
                Name = entityType.Name,
                Apperance = entityType.ColorHex,
                Active = entityType.IsActive,
                LastUser = userName,
                LastChange = DateTime.UtcNow
            };
            context.EntityTypes.Add(et);
            await context.SaveChangesAsync();
            entityType.Id = et.EntityTypeId;
            return entityType;
        }

        public async Task<IEnumerable<IdNameDTO>> GetEntitiesIdNameAsync(bool activeOnly)
        {
            List<IdNameDTO> entities = new List<IdNameDTO>();
            using var context = new AppOverviewContext(_connetionString);
            var entitiesQry = context.Entities.Where(x => !activeOnly || x.Active);
            await entitiesQry.ForEachAsync(entity =>
                entities.Add(new IdNameDTO
                {
                    Id = entity.EntityId,
                    Name = entity.Name,
                    IsActive = entity.Active
                }));
            return entities;
        }

        public async Task<IEnumerable<IdNameDTO>> GetEntityTypeIdNameAsync(bool activeOnly)
        {
            List<IdNameDTO> entityTypes = new List<IdNameDTO>();
            using var context = new AppOverviewContext(_connetionString);
            var entityTypesQry = context.EntityTypes.Where(x => !activeOnly || x.Active);
            await entityTypesQry.ForEachAsync(entityType =>
                entityTypes.Add(new IdNameDTO
                {
                    Id = entityType.EntityTypeId,
                    Name = entityType.Name,
                    IsActive = entityType.Active
                }));
            return entityTypes;
        }

        private async Task<IEnumerable<EntityDTO>> LoadDependenciesAsync(int entityId)
        {
            List<EntityDTO> dependencies = new List<EntityDTO>();
            using var context = new AppOverviewContext(_connetionString);
            var dependenciesQry = context.Relations
                .Where(r => r.SourceEntityId == entityId && r.TargetEntity.Active)
                .Select(r => r.TargetEntity);
            await dependenciesQry.ForEachAsync(dependency =>
            {
                dependencies.Add(new EntityDTO
                {
                    Id = dependency.EntityId,
                    Name = dependency.Name,
                    Description = dependency.Description ?? string.Empty,
                    TypeId = dependency.EntityTypeId,
                    Type = dependency.EntityType?.Name ?? string.Empty,
                    DepartmentId = dependency.DepartmentId,
                    Department = dependency.Department?.Name ?? string.Empty,
                    TechnologyId = dependency.TechnologyId,
                    Technology = dependency.Technology?.Name ?? string.Empty,
                    SourceControlUrl = dependency.SourceControlUrl ?? string.Empty,
                    Owner = dependency.Owner ?? string.Empty,
                    ColorHex = dependency.EntityType?.Apperance ?? "#000000",
                    IsActive = dependency.Active
                });
            });
            return dependencies;
        }
    }
}
