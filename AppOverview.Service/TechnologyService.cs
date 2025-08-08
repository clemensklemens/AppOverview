using AppOverview.Model;
using AppOverview.Model.DTOs;
using AppOverview.Model.Interfaces;
using Microsoft.Extensions.Logging;

namespace AppOverview.Service
{
    public class TechnologyService(IDataProvider dataProvider, ILogger<TechnologyService> logger) : ITechnologyService
    {
        private readonly IDataProvider _dataProvider = dataProvider;
        private readonly ILogger<TechnologyService> _logger = logger;

        public async Task<IdNameDTO> AddTechnologyAsync(IdNameDTO technology, string userName)
        {
            if (string.IsNullOrWhiteSpace(technology.Name))
            {
                string errorMessage = "Technology name cannot be null or empty.";
                var ex = new ArgumentException(errorMessage, nameof(technology));
                _logger.LogError("{ErrorMessage} Technology: {Technology}", errorMessage, ex);
                throw ex;
            }

            try
            {
                var newTechnology = await _dataProvider.AddTechnologyAsync(technology, userName);
                return newTechnology;
            }
            catch(Exception ex)
            {
                string errorMessage = $"An error occurred while adding the technology: {technology.Name}";
                _logger.LogError("{ErrorMessage} Exception: {Exception}", errorMessage, ex);
                throw new ServiceException(errorMessage, ex);
            }   
        }

        public async Task<IEnumerable<IdNameDTO>> GetTechnologiesAsync()
        {
            try
            {
                return await _dataProvider.GetTechnologiesAsync(false);
            }
            catch (Exception ex)
            {
                string errorMessage = $"An error occurred while retrieving technologies";
                _logger.LogError("{ErrorMessage} Exception: {Exception}", errorMessage, ex);
                throw new ServiceException(errorMessage, ex);
            }
        }

        public async Task UpdateTechnologyAsync(IdNameDTO technology, string userName)
        {
            if (string.IsNullOrWhiteSpace(technology.Name))
            {
                string errorMessage = "Technology name cannot be null or empty.";
                var ex = new ArgumentException(errorMessage, nameof(technology));
                _logger.LogError("{ErrorMessage} Technology: {Technology}", errorMessage, ex);
                throw ex;
            }

            try
            {
                await _dataProvider.UpdateTechnologyAsync(technology, userName);
            }
            catch (Exception ex)
            {
                string errorMessage = $"An error occurred while updating the technology: {technology.Name}";
                _logger.LogError("{ErrorMessage} Exception: {Exception}", errorMessage, ex);
                throw new ServiceException(errorMessage, ex);
            }
        }
    }
}
