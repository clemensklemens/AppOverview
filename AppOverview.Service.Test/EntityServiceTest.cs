using AppOverview.Model.DTOs;
using AppOverview.Model.Interfaces;
using AppOverview.Model;
using Microsoft.Extensions.Logging;
using Moq;

namespace AppOverview.Service.Test
{
    public class EntityServiceTest
    {
        private readonly Mock<IDataProvider> _dataProviderMock = new();
        private readonly Mock<ILogger<EntityService>> _loggerMock = new();
        private readonly EntityService _service;

        public EntityServiceTest()
        {
            _service = new EntityService(_dataProviderMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task AddEntityAsync_ValidEntity_ReturnsNewEntity()
        {
            var entity = new EntityDTO { Name = "Test", TypeId = 1, DepartmentId = 1, TechnologyId = 1 };
            var expected = new EntityDTO { Id = 1, Name = "Test", TypeId = 1, DepartmentId = 1, TechnologyId = 1 };
            _dataProviderMock.Setup(x => x.AddEntityAsync(entity, "user")).ReturnsAsync(expected);

            var result = await _service.AddEntityAsync(entity, "user");

            Assert.Equal(expected.Id, result.Id);
            Assert.Equal(expected.Name, result.Name);
        }

        [Fact]
        public async Task AddEntityAsync_InvalidEntity_ThrowsArgumentException()
        {
            var entity = new EntityDTO { Name = "", TypeId = 0, DepartmentId = 0, TechnologyId = 0 };
            await Assert.ThrowsAsync<ArgumentException>(() => _service.AddEntityAsync(entity, "user"));
        }

        [Fact]
        public async Task AddEntityAsync_DataProviderThrows_ThrowsServiceException()
        {
            var entity = new EntityDTO { Name = "Test", TypeId = 1, DepartmentId = 1, TechnologyId = 1 };
            _dataProviderMock.Setup(x => x.AddEntityAsync(entity, "user")).ThrowsAsync(new System.Exception("fail"));
            await Assert.ThrowsAsync<ServiceException>(() => _service.AddEntityAsync(entity, "user"));
        }

        [Fact]
        public async Task AddRelatedEntityAsync_AddsRelationIfNotExists()
        {
            var entity = new EntityDTO { Id = 1, Name = "A", TypeId = 1, DepartmentId = 1, TechnologyId = 1 };
            var related = new EntityDTO { Id = 2, Name = "B", TypeId = 1, DepartmentId = 1, TechnologyId = 1 };
            _dataProviderMock.Setup(x => x.GetEntityAsync(1)).ReturnsAsync(entity);
            _dataProviderMock.Setup(x => x.GetEntityAsync(2)).ReturnsAsync(related);
            _dataProviderMock.Setup(x => x.UpdateEntityAsync(entity, "user")).Returns(Task.CompletedTask);

            var result = await _service.AddRelatedEntityAsync(1, 2, "user");

            Assert.Contains(related, entity.Dependencies);
        }

        [Fact]
        public async Task AddRelatedEntityAsync_DoesNotAddIfAlreadyRelated()
        {
            var related = new EntityDTO { Id = 2, Name = "B", TypeId = 1, DepartmentId = 1, TechnologyId = 1 };
            var entity = new EntityDTO { Id = 1, Name = "A", TypeId = 1, DepartmentId = 1, TechnologyId = 1, Dependencies = new List<EntityDTO> { related } };
            _dataProviderMock.Setup(x => x.GetEntityAsync(1)).ReturnsAsync(entity);
            _dataProviderMock.Setup(x => x.GetEntityAsync(2)).ReturnsAsync(related);
            _dataProviderMock.Setup(x => x.UpdateEntityAsync(entity, "user")).Returns(Task.CompletedTask);

            var result = await _service.AddRelatedEntityAsync(1, 2, "user");

            Assert.Single(entity.Dependencies);
        }

        [Fact]
        public async Task AddRelatedEntityAsync_DataProviderThrows_ThrowsServiceException()
        {
            _dataProviderMock.Setup(x => x.GetEntityAsync(It.IsAny<int>())).ThrowsAsync(new System.Exception("fail"));
            await Assert.ThrowsAsync<ServiceException>(() => _service.AddRelatedEntityAsync(1, 2, "user"));
        }

        [Fact]
        public async Task RemoveRelatedEntityAsync_RemovesRelationIfExists()
        {
            var related = new EntityDTO { Id = 2, Name = "B", TypeId = 1, DepartmentId = 1, TechnologyId = 1 };
            var entity = new EntityDTO { Id = 1, Name = "A", TypeId = 1, DepartmentId = 1, TechnologyId = 1, Dependencies = new List<EntityDTO> { related } };
            _dataProviderMock.Setup(x => x.GetEntityAsync(1)).ReturnsAsync(entity);
            _dataProviderMock.Setup(x => x.GetEntityAsync(2)).ReturnsAsync(related);
            _dataProviderMock.Setup(x => x.UpdateEntityAsync(entity, "user")).Returns(Task.CompletedTask);

            var result = await _service.RemoveRelatedEntityAsync(1, 2, "user");

            Assert.DoesNotContain(related, entity.Dependencies);
        }

        [Fact]
        public async Task RemoveRelatedEntityAsync_DoesNothingIfNotRelated()
        {
            var related = new EntityDTO { Id = 2, Name = "B", TypeId = 1, DepartmentId = 1, TechnologyId = 1 };
            var entity = new EntityDTO { Id = 1, Name = "A", TypeId = 1, DepartmentId = 1, TechnologyId = 1, Dependencies = new List<EntityDTO>() };
            _dataProviderMock.Setup(x => x.GetEntityAsync(1)).ReturnsAsync(entity);
            _dataProviderMock.Setup(x => x.GetEntityAsync(2)).ReturnsAsync(related);
            _dataProviderMock.Setup(x => x.UpdateEntityAsync(entity, "user")).Returns(Task.CompletedTask);

            var result = await _service.RemoveRelatedEntityAsync(1, 2, "user");

            Assert.Empty(entity.Dependencies);
        }

        [Fact]
        public async Task RemoveRelatedEntityAsync_DataProviderThrows_ThrowsServiceException()
        {
            _dataProviderMock.Setup(x => x.GetEntityAsync(It.IsAny<int>())).ThrowsAsync(new System.Exception("fail"));
            await Assert.ThrowsAsync<ServiceException>(() => _service.RemoveRelatedEntityAsync(1, 2, "user"));
        }

        [Fact]
        public async Task UpdateEntityAsync_ValidEntity_CallsUpdate()
        {
            var entity = new EntityDTO { Id = 1, Name = "A", TypeId = 1, DepartmentId = 1, TechnologyId = 1 };
            _dataProviderMock.Setup(x => x.UpdateEntityAsync(entity, "user")).Returns(Task.CompletedTask);
            await _service.UpdateEntityAsync(entity, "user");
            _dataProviderMock.Verify(x => x.UpdateEntityAsync(entity, "user"), Times.Once);
        }

        [Fact]
        public async Task UpdateEntityAsync_InvalidEntity_ThrowsArgumentException()
        {
            var entity = new EntityDTO { Id = 1, Name = "", TypeId = 0, DepartmentId = 0, TechnologyId = 0 };
            await Assert.ThrowsAsync<ArgumentException>(() => _service.UpdateEntityAsync(entity, "user"));
        }

        [Fact]
        public async Task UpdateEntityAsync_DataProviderThrows_ThrowsServiceException()
        {
            var entity = new EntityDTO { Id = 1, Name = "A", TypeId = 1, DepartmentId = 1, TechnologyId = 1 };
            _dataProviderMock.Setup(x => x.UpdateEntityAsync(entity, "user")).ThrowsAsync(new System.Exception("fail"));
            await Assert.ThrowsAsync<ServiceException>(() => _service.UpdateEntityAsync(entity, "user"));
        }

        [Fact]
        public async Task GetEntitiesAsync_ReturnsEntities()
        {
            var entities = new List<EntityDTO> { new EntityDTO { Id = 1, Name = "A", TypeId = 1, DepartmentId = 1, TechnologyId = 1 } };
            _dataProviderMock.Setup(x => x.GetEntitiesAsync(false)).ReturnsAsync(entities);
            var result = await _service.GetEntitiesAsync();
            Assert.Single(result);
        }

        [Fact]
        public async Task GetEntitiesAsync_DataProviderThrows_ThrowsServiceException()
        {
            _dataProviderMock.Setup(x => x.GetEntitiesAsync(false)).ThrowsAsync(new System.Exception("fail"));
            await Assert.ThrowsAsync<ServiceException>(() => _service.GetEntitiesAsync());
        }

        [Fact]
        public async Task GetDepartmentsAsync_ReturnsDepartments()
        {
            var departments = new List<IdNameDTO> { new IdNameDTO(1, "Dept") };
            _dataProviderMock.Setup(x => x.GetDepartmentsIdNameListAsync(true)).ReturnsAsync(departments);
            var result = await _service.GetDepartmentsAsync();
            Assert.Single(result);
        }

        [Fact]
        public async Task GetDepartmentsAsync_DataProviderThrows_ThrowsServiceException()
        {
            _dataProviderMock.Setup(x => x.GetDepartmentsIdNameListAsync(true)).ThrowsAsync(new System.Exception("fail"));
            await Assert.ThrowsAsync<ServiceException>(() => _service.GetDepartmentsAsync());
        }

        [Fact]
        public async Task GetTechnologiesAsync_ReturnsTechnologies()
        {
            var technologies = new List<IdNameDTO> { new IdNameDTO(1, "Tech") };
            _dataProviderMock.Setup(x => x.GetTechnologiesIdNameListAsync(true)).ReturnsAsync(technologies);
            var result = await _service.GetTechnologiesAsync();
            Assert.Single(result);
        }

        [Fact]
        public async Task GetTechnologiesAsync_DataProviderThrows_ThrowsServiceException()
        {
            _dataProviderMock.Setup(x => x.GetTechnologiesIdNameListAsync(true)).ThrowsAsync(new System.Exception("fail"));
            await Assert.ThrowsAsync<ServiceException>(() => _service.GetTechnologiesAsync());
        }

        [Fact]
        public async Task GetEntityTypesAsync_ReturnsEntityTypes()
        {
            var types = new List<IdNameDTO> { new IdNameDTO(1, "Type") };
            _dataProviderMock.Setup(x => x.GetEntityTypeIdNameListAsync(true)).ReturnsAsync(types);
            var result = await _service.GetEntityTypesAsync();
            Assert.Single(result);
        }

        [Fact]
        public async Task GetEntityTypesAsync_DataProviderThrows_ThrowsServiceException()
        {
            _dataProviderMock.Setup(x => x.GetEntityTypeIdNameListAsync(true)).ThrowsAsync(new System.Exception("fail"));
            await Assert.ThrowsAsync<ServiceException>(() => _service.GetEntityTypesAsync());
        }
    }
}