using AppOverview.Model;

namespace AppOverview.Service
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDataProvider _dataProvider;

        public DepartmentService(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public async Task<DepartmentDTO> AddDepartmentAsync(DepartmentDTO department)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<DepartmentDTO>> GetAllDepartmentsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<DepartmentDTO> GetDepartmentAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateDepartmentAsync(DepartmentDTO department)
        {
            throw new NotImplementedException();
        }
    }
}
