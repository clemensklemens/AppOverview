using AppOverview.Model;
using Microsoft.Extensions.Logging;

namespace AppOverview.Service
{
    public class DepartmentService(IDataProvider dataProvider, ILogger<DepartmentService> logger) : IDepartmentService
    {
        private readonly IDataProvider _dataProvider = dataProvider;
        private readonly ILogger<DepartmentService> _logger = logger;

        public async Task<DepartmentDTO> AddDepartmentAsync(DepartmentDTO department)
        {
            try
            {
                if(string.IsNullOrWhiteSpace(department.Name))
                {
                    throw new ArgumentException("Department name cannot be null or empty.", nameof(department));
                }
                var newDepartment = await _dataProvider.AddDepartmentAsync(department);
                return newDepartment;
            }
            catch(Exception ex)
            {
                string errorMessage = $"An error occurred while adding department: {department.Name}";
                _logger.LogError("{ErrorMessage} Exception: {Exception}", errorMessage, ex);
                throw new ServiceException(errorMessage, ex);
            }
        }

        public async Task<IEnumerable<DepartmentDTO>> GetDepartmentsAsync()
        {
            try
            {                
                return await _dataProvider.GetDepartmentsAsync();
            }
            catch(Exception ex)
            {
                string errorMessage = "An error occurred while retrieving departments.";
                _logger.LogError("{ErrorMessage} Exception: {Exception}", errorMessage, ex);
                throw new ServiceException(errorMessage, ex);
            }
        }

        public async Task UpdateDepartmentAsync(DepartmentDTO department)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(department.Name))
                {
                    throw new ArgumentException("Department name cannot be null or empty.", nameof(department));
                }
                await _dataProvider.UpdateDepartmentAsync(department);
            }
            catch (Exception ex)
            {
                string errorMessage = $"An error occurred while updating department: {department.Name}";
                _logger.LogError("{ErrorMessage} Exception: {Exception}", errorMessage, ex);
                throw new ServiceException(errorMessage, ex);
            }
        }
    }
}
