using AppOverview.Model.DTOs;
using AppOverview.Model.Interfaces;
using AppOverview.Model;
using Microsoft.Extensions.Logging;
using Moq;

namespace AppOverview.Service.Test
{
    public class TechnologyServiceTest
    {
        private readonly Mock<IDataProvider> _dataProviderMock;
        private readonly Mock<ILogger<TechnologyService>> _loggerMock;
        private readonly TechnologyService _service;

        public TechnologyServiceTest()
        {
            _dataProviderMock = new Mock<IDataProvider>();
            _loggerMock = new Mock<ILogger<TechnologyService>>();
            _service = new TechnologyService(_dataProviderMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task AddTechnologyAsync_ValidTechnology_ReturnsTechnology()
        {
            var technology = new TechnologyDTO { Id = 1, Name = ".NET", IsActive = true };
            _dataProviderMock.Setup(x => x.AddTechnologyAsync(technology, "user")).ReturnsAsync(technology);

            var result = await _service.AddTechnologyAsync(technology, "user");

            Assert.Equal(technology, result);
        }

        [Fact]
        public async Task AddTechnologyAsync_EmptyName_ThrowsArgumentException()
        {
            var technology = new TechnologyDTO { Id = 1, Name = "", IsActive = true };
            await Assert.ThrowsAsync<ArgumentException>(() => _service.AddTechnologyAsync(technology, "user"));
        }

        [Fact]
        public async Task AddTechnologyAsync_DataProviderThrows_ThrowsServiceException()
        {
            var technology = new TechnologyDTO { Id = 1, Name = ".NET", IsActive = true };
            _dataProviderMock.Setup(x => x.AddTechnologyAsync(technology, "user")).ThrowsAsync(new Exception("db error"));

            var ex = await Assert.ThrowsAsync<ServiceException>(() => _service.AddTechnologyAsync(technology, "user"));
            Assert.Contains("An error occurred while adding the technology", ex.Message);
        }

        [Fact]
        public async Task GetTechnologiesAsync_ReturnsTechnologies()
        {
            var technologies = new List<TechnologyDTO> { new TechnologyDTO { Id = 1, Name = ".NET" } };
            _dataProviderMock.Setup(x => x.GetTechnologiesAsync(false)).ReturnsAsync(technologies);

            var result = await _service.GetTechnologiesAsync();

            Assert.Single(result);
        }

        [Fact]
        public async Task GetTechnologiesAsync_DataProviderThrows_ThrowsServiceException()
        {
            _dataProviderMock.Setup(x => x.GetTechnologiesAsync(false)).ThrowsAsync(new Exception("db error"));
            var ex = await Assert.ThrowsAsync<ServiceException>(() => _service.GetTechnologiesAsync());
            Assert.Contains("An error occurred while retrieving technologies", ex.Message);
        }

        [Fact]
        public async Task UpdateTechnologyAsync_ValidTechnology_CallsDataProvider()
        {
            var technology = new TechnologyDTO { Id = 1, Name = ".NET", IsActive = true };
            _dataProviderMock.Setup(x => x.UpdateTechnologyAsync(technology, "user")).Returns(Task.CompletedTask);

            await _service.UpdateTechnologyAsync(technology, "user");
            _dataProviderMock.Verify(x => x.UpdateTechnologyAsync(technology, "user"), Times.Once);
        }

        [Fact]
        public async Task UpdateTechnologyAsync_EmptyName_ThrowsArgumentException()
        {
            var technology = new TechnologyDTO { Id = 1, Name = "", IsActive = true };
            await Assert.ThrowsAsync<ArgumentException>(() => _service.UpdateTechnologyAsync(technology, "user"));
        }

        [Fact]
        public async Task UpdateTechnologyAsync_DataProviderThrows_ThrowsServiceException()
        {
            var technology = new TechnologyDTO { Id = 1, Name = ".NET", IsActive = true };
            _dataProviderMock.Setup(x => x.UpdateTechnologyAsync(technology, "user")).ThrowsAsync(new Exception("db error"));

            var ex = await Assert.ThrowsAsync<ServiceException>(() => _service.UpdateTechnologyAsync(technology, "user"));
            Assert.Contains("An error occurred while updating the technology", ex.Message);
        }
    }
}