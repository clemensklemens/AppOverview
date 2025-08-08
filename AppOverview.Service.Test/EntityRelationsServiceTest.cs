using AppOverview.Model.DTOs;
using AppOverview.Model.Interfaces;
using AppOverview.Model;
using Microsoft.Extensions.Logging;
using Moq;

namespace AppOverview.Service.Test
{
    public class EntityRelationsServiceTest
    {
        private readonly Mock<IDataProvider> _dataProviderMock = new();
        private readonly Mock<ILogger<EntityRelationsService>> _loggerMock = new();
        private readonly EntityRelationsService _service;

        public EntityRelationsServiceTest()
        {
            _service = new EntityRelationsService(_dataProviderMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task InitServiceAsync_LoadsEntitiesTypesDepartments()
        {
            var entities = new List<EntityDTO>
            {
                new EntityDTO { Id = 1, Name = "A", TypeId = 1, Type = "Type1", DepartmentId = 1, Department = "Dept1" },
                new EntityDTO { Id = 2, Name = "B", TypeId = 2, Type = "Type2", DepartmentId = 2, Department = "Dept2" }
            };
            _dataProviderMock.Setup(x => x.GetEntitiesAsync(true)).ReturnsAsync(entities);

            await _service.InitServiceAsync();

            Assert.Equal(2, _service.EntityTypes.Count());
            Assert.Equal(2, _service.Departments.Count());
        }

        [Fact]
        public async Task InitServiceAsync_DataProviderThrows_ThrowsServiceException()
        {
            _dataProviderMock.Setup(x => x.GetEntitiesAsync(true)).ThrowsAsync(new System.Exception("fail"));
            await Assert.ThrowsAsync<ServiceException>(() => _service.InitServiceAsync());
        }

        [Fact]
        public async Task FilterEntities_FiltersByNameTypeDepartment()
        {
            var entities = new List<EntityDTO>
            {
                new EntityDTO { Id = 1, Name = "Alpha", TypeId = 1, Type = "Type1", DepartmentId = 1, Department = "Dept1" },
                new EntityDTO { Id = 2, Name = "Beta", TypeId = 2, Type = "Type2", DepartmentId = 2, Department = "Dept2" }
            };
            _dataProviderMock.Setup(x => x.GetEntitiesAsync(true)).ReturnsAsync(entities);
            await _service.InitServiceAsync();

            _service.FilterEntities("Alpha", new HashSet<int>(), new HashSet<int>());
            var (nodes, _) = _service.GetGraphItems();
            Assert.Single(nodes);
            Assert.Equal("Alpha", nodes.First().label);

            _service.FilterEntities(null, new HashSet<int> { 2 }, new HashSet<int>());
            (nodes, _) = _service.GetGraphItems();
            Assert.Single(nodes);
            Assert.Equal("Beta", nodes.First().label);

            _service.FilterEntities(null, new HashSet<int>(), new HashSet<int> { 1 });
            (nodes, _) = _service.GetGraphItems();
            Assert.Single(nodes);
            Assert.Equal("Alpha", nodes.First().label);
        }

        [Fact]
        public async Task FilterEntities_NameFilterIncludesRelatedEntities()
        {
            var dep = new EntityDTO { Id = 2, Name = "Beta", TypeId = 2, Type = "Type2", DepartmentId = 2, Department = "Dept2" };
            var entity = new EntityDTO { Id = 1, Name = "Alpha", TypeId = 1, Type = "Type1", DepartmentId = 1, Department = "Dept1", Dependencies = new List<EntityDTO> { dep } };
            var entities = new List<EntityDTO> { entity, dep };
            _dataProviderMock.Setup(x => x.GetEntitiesAsync(true)).ReturnsAsync(entities);
            await _service.InitServiceAsync();

            _service.FilterEntities("Alpha", new HashSet<int>(), new HashSet<int>());
            var (nodes, _) = _service.GetGraphItems();
            Assert.Equal(2, nodes.Count());
            Assert.Contains(nodes, n => n.label == "Alpha");
            Assert.Contains(nodes, n => n.label == "Beta");
        }

        [Fact]
        public async Task FilterEntities_ForEmptyList_SetsToEmptyList()
        {
            await _service.InitServiceAsync();
            _service.FilterEntities(null, new HashSet<int>(), new HashSet<int>());

            // Use reflection to check that _filteredEntities is an empty list
            var field = typeof(EntityRelationsService).GetField("_filteredEntities", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var value = field?.GetValue(_service) as List<EntityDTO>;
            Assert.NotNull(value);
            Assert.Empty(value);
        }

        [Fact]
        public async Task GetGraphItems_ReturnsCorrectEdges()
        {
            var dep = new EntityDTO { Id = 2, Name = "Beta", TypeId = 2, Type = "Type2", DepartmentId = 2, Department = "Dept2" };
            var entity = new EntityDTO { Id = 1, Name = "Alpha", TypeId = 1, Type = "Type1", DepartmentId = 1, Department = "Dept1", Dependencies = new List<EntityDTO> { dep } };
            var entities = new List<EntityDTO> { entity, dep };
            _dataProviderMock.Setup(x => x.GetEntitiesAsync(true)).ReturnsAsync(entities);
            await _service.InitServiceAsync();
            _service.FilterEntities(null, new HashSet<int>(), new HashSet<int>());

            var (nodes, edges) = _service.GetGraphItems();
            Assert.Equal(2, nodes.Count());
            Assert.Single(edges);
            Assert.Equal(entity.Id, edges.First().from);
            Assert.Equal(dep.Id, edges.First().to);
        }
    }
}