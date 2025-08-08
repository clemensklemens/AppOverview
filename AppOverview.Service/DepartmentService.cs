using AppOverview.Model;
using AppOverview.Model.DTOs;
using AppOverview.Model.Interfaces;
using Microsoft.Extensions.Logging;

namespace AppOverview.Service
{
    public class DepartmentService(IDataProvider dataProvider, ILogger<DepartmentService> logger) : IDepartmentService
    {
        private readonly IDataProvider _dataProvider = dataProvider;
        private readonly ILogger<DepartmentService> _logger = logger;

        public async Task<DepartmentDTO> AddDepartmentAsync(DepartmentDTO department, string userName)
        {
            if (string.IsNullOrWhiteSpace(department.Name))
            {
                string errorMessage = "Department name cannot be null or empty.";
                var ex = new ArgumentException(errorMessage, nameof(department));
                _logger.LogError("{ErrorMessage} Exception: {Exception}", errorMessage, ex);
                throw ex;
            }

            try
            {
                var newDepartment = await _dataProvider.AddDepartmentAsync(department, userName);
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
                return await _dataProvider.GetDepartmentsAsync(false);
            }
            catch(Exception ex)
            {
                string errorMessage = "An error occurred while retrieving departments.";
                _logger.LogError("{ErrorMessage} Exception: {Exception}", errorMessage, ex);
                throw new ServiceException(errorMessage, ex);
            }
        }

        public async Task UpdateDepartmentAsync(DepartmentDTO department, string userName)
        {
            if (string.IsNullOrWhiteSpace(department.Name))
            {
                string errorMessage = "Department name cannot be null or empty.";
                var ex = new ArgumentException(errorMessage, nameof(department));
                _logger.LogError("{ErrorMessage} Exception: {Exception}", errorMessage, ex);
                throw ex;
            }

            try
            {
                await _dataProvider.UpdateDepartmentAsync(department, userName);
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
