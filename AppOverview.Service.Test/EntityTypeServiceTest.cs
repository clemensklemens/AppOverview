using AppOverview.Model.DTOs;
using AppOverview.Model.Interfaces;
using AppOverview.Model;
using Microsoft.Extensions.Logging;
using Moq;

namespace AppOverview.Service.Test
{
    public class EntityTypeServiceTest
    {
        private readonly Mock<IDataProvider> _dataProviderMock;
        private readonly Mock<ILogger<EntityTypeService>> _loggerMock;
        private readonly EntityTypeService _service;

        public EntityTypeServiceTest()
        {
            _dataProviderMock = new Mock<IDataProvider>();
            _loggerMock = new Mock<ILogger<EntityTypeService>>();
            _service = new EntityTypeService(_dataProviderMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task AddEntityTypeAsync_ValidEntityType_ReturnsEntityType()
        {
            var entityType = new EntityTypeDTO { Id = 1, Name = "Type1", IsActive = true };
            _dataProviderMock.Setup(x => x.AddEntityTypeAsync(entityType, "user")).ReturnsAsync(entityType);

            var result = await _service.AddEntityTypeAsync(entityType, "user");

            Assert.Equal(entityType, result);
        }

        [Fact]
        public async Task AddEntityTypeAsync_EmptyName_ThrowsArgumentException()
        {
            var entityType = new EntityTypeDTO { Id = 1, Name = "", IsActive = true };
            await Assert.ThrowsAsync<ArgumentException>(() => _service.AddEntityTypeAsync(entityType, "user"));
        }

        [Fact]
        public async Task AddEntityTypeAsync_DataProviderThrows_ThrowsServiceException()
        {
            var entityType = new EntityTypeDTO { Id = 1, Name = "Type1", IsActive = true };
            _dataProviderMock.Setup(x => x.AddEntityTypeAsync(entityType, "user")).ThrowsAsync(new Exception("db error"));

            var ex = await Assert.ThrowsAsync<ServiceException>(() => _service.AddEntityTypeAsync(entityType, "user"));
            Assert.Contains("An error occurred while adding the entity type", ex.Message);
        }

        [Fact]
        public async Task GetEntityTypesAsync_ReturnsEntityTypes()
        {
            var entityTypes = new List<EntityTypeDTO> { new EntityTypeDTO { Id = 1, Name = "Type1" } };
            _dataProviderMock.Setup(x => x.GetEntityTypesAsync(false)).ReturnsAsync(entityTypes);

            var result = await _service.GetEntityTypesAsync();

            Assert.Single(result);
        }

        [Fact]
        public async Task GetEntityTypesAsync_DataProviderThrows_ThrowsServiceException()
        {
            _dataProviderMock.Setup(x => x.GetEntityTypesAsync(false)).ThrowsAsync(new Exception("db error"));
            var ex = await Assert.ThrowsAsync<ServiceException>(() => _service.GetEntityTypesAsync());
            Assert.Contains("An error occurred while retrieving entity types", ex.Message);
        }

        [Fact]
        public async Task UpdateEntityTypeAsync_ValidEntityType_CallsDataProvider()
        {
            var entityType = new EntityTypeDTO { Id = 1, Name = "Type1", IsActive = true };
            _dataProviderMock.Setup(x => x.UpdateEntityTypeAsync(entityType, "user")).Returns(Task.CompletedTask);

            await _service.UpdateEntityTypeAsync(entityType, "user");
            _dataProviderMock.Verify(x => x.UpdateEntityTypeAsync(entityType, "user"), Times.Once);
        }

        [Fact]
        public async Task UpdateEntityTypeAsync_EmptyName_ThrowsArgumentException()
        {
            var entityType = new EntityTypeDTO { Id = 1, Name = "", IsActive = true };
            await Assert.ThrowsAsync<ArgumentException>(() => _service.UpdateEntityTypeAsync(entityType, "user"));
        }

        [Fact]
        public async Task UpdateEntityTypeAsync_DataProviderThrows_ThrowsServiceException()
        {
            var entityType = new EntityTypeDTO { Id = 1, Name = "Type1", IsActive = true };
            _dataProviderMock.Setup(x => x.UpdateEntityTypeAsync(entityType, "user")).ThrowsAsync(new Exception("db error"));

            var ex = await Assert.ThrowsAsync<ServiceException>(() => _service.UpdateEntityTypeAsync(entityType, "user"));
            Assert.Contains("An error occurred while updating the entity type", ex.Message);
        }
    }
}