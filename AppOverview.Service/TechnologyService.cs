using AppOverview.Model;
using Microsoft.Extensions.Logging;

namespace AppOverview.Service
{
    public class TechnologyService(IDataProvider dataProvider, ILogger<TechnologyService> logger) : ITechnologyService
    {
        private readonly IDataProvider _dataProvider = dataProvider;
        private readonly ILogger<TechnologyService> _logger = logger;

        public async Task<TechnologyDTO> AddTechnologyAsync(TechnologyDTO technology)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(technology.Name))
                {
                    throw new ArgumentException("Technology name cannot be null or empty.", nameof(technology));
                }
                var newTechnology = await _dataProvider.AddTechnologyAsync(technology);
                return newTechnology;
            }
            catch(Exception ex)
            {
                string errorMessage = $"An error occurred while adding the technology: {technology.Name}";
                _logger.LogError("{ErrorMessage} Exception: {Exception}", errorMessage, ex);
                throw new ServiceException(errorMessage, ex);
            }   
        }

        public async Task<IEnumerable<TechnologyDTO>> GetTechnologiesAsync()
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

        public async Task UpdateTechnologyAsync(TechnologyDTO technology)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(technology.Name))
                {
                   throw new ArgumentException("Technology name cannot be null or empty.", nameof(technology));
                }
                await _dataProvider.UpdateTechnologyAsync(technology);
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
