using AppOverview.Model.DTOs;

namespace AppOverview.Model.Interfaces
{
    public interface ITechnologyService
    {
        Task<TechnologyDTO> AddTechnologyAsync(TechnologyDTO technology, string userName);
        Task<IEnumerable<TechnologyDTO>> GetTechnologiesAsync();
        Task UpdateTechnologyAsync(TechnologyDTO technology, string userName);
    }
}
