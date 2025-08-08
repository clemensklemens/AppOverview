using AppOverview.Model.DTOs;
using AppOverview.Model.Interfaces;
using AppOverview.Model;
using Microsoft.Extensions.Logging;
using Moq;

namespace AppOverview.Service.Test
{
    public class DepartmenServiceTest
    {
        private readonly Mock<IDataProvider> _dataProviderMock;
        private readonly Mock<ILogger<DepartmentService>> _loggerMock;
        private readonly DepartmentService _service;

        public DepartmenServiceTest()
        {
            _dataProviderMock = new Mock<IDataProvider>();
            _loggerMock = new Mock<ILogger<DepartmentService>>();
            _service = new DepartmentService(_dataProviderMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task AddDepartmentAsync_ValidDepartment_ReturnsDepartment()
        {
            var department = new DepartmentDTO { Id = 1, Name = "HR", IsActive = true };
            _dataProviderMock.Setup(x => x.AddDepartmentAsync(department, "user")).ReturnsAsync(department);

            var result = await _service.AddDepartmentAsync(department, "user");

            Assert.Equal(department, result);
        }

        [Fact]
        public async Task AddDepartmentAsync_EmptyName_ThrowsArgumentException()
        {
            var department = new DepartmentDTO { Id = 1, Name = "", IsActive = true };
            await Assert.ThrowsAsync<ArgumentException>(() => _service.AddDepartmentAsync(department, "user"));
        }

        [Fact]
        public async Task AddDepartmentAsync_DataProviderThrows_ThrowsServiceException()
        {
            var department = new DepartmentDTO { Id = 1, Name = "HR", IsActive = true };
            _dataProviderMock.Setup(x => x.AddDepartmentAsync(department, "user")).ThrowsAsync(new Exception("db error"));

            var ex = await Assert.ThrowsAsync<ServiceException>(() => _service.AddDepartmentAsync(department, "user"));
            Assert.Contains("An error occurred while adding department", ex.Message);
        }

        [Fact]
        public async Task GetDepartmentsAsync_ReturnsDepartments()
        {
            var departments = new List<DepartmentDTO> { new DepartmentDTO { Id = 1, Name = "HR" } };
            _dataProviderMock.Setup(x => x.GetDepartmentsAsync(false)).ReturnsAsync(departments);

            var result = await _service.GetDepartmentsAsync();

            Assert.Single(result);
        }

        [Fact]
        public async Task GetDepartmentsAsync_DataProviderThrows_ThrowsServiceException()
        {
            _dataProviderMock.Setup(x => x.GetDepartmentsAsync(false)).ThrowsAsync(new Exception("db error"));
            var ex = await Assert.ThrowsAsync<ServiceException>(() => _service.GetDepartmentsAsync());
            Assert.Contains("An error occurred while retrieving departments", ex.Message);
        }

        [Fact]
        public async Task UpdateDepartmentAsync_ValidDepartment_CallsDataProvider()
        {
            var department = new DepartmentDTO { Id = 1, Name = "HR", IsActive = true };
            _dataProviderMock.Setup(x => x.UpdateDepartmentAsync(department, "user")).Returns(Task.CompletedTask);

            await _service.UpdateDepartmentAsync(department, "user");
            _dataProviderMock.Verify(x => x.UpdateDepartmentAsync(department, "user"), Times.Once);
        }

        [Fact]
        public async Task UpdateDepartmentAsync_EmptyName_ThrowsArgumentException()
        {
            var department = new DepartmentDTO { Id = 1, Name = "", IsActive = true };
            await Assert.ThrowsAsync<ArgumentException>(() => _service.UpdateDepartmentAsync(department, "user"));
        }

        [Fact]
        public async Task UpdateDepartmentAsync_DataProviderThrows_ThrowsServiceException()
        {
            var department = new DepartmentDTO { Id = 1, Name = "HR", IsActive = true };
            _dataProviderMock.Setup(x => x.UpdateDepartmentAsync(department, "user")).ThrowsAsync(new Exception("db error"));

            var ex = await Assert.ThrowsAsync<ServiceException>(() => _service.UpdateDepartmentAsync(department, "user"));
            Assert.Contains("An error occurred while updating department", ex.Message);
        }
    }
}