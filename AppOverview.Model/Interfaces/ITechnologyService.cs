using AppOverview.Model.DTOs;

namespace AppOverview.Model.Interfaces
{
    public interface ITechnologyService
    {
        Task<IdNameDTO> AddTechnologyAsync(IdNameDTO technology, string userName);
        Task<IEnumerable<IdNameDTO>> GetTechnologiesAsync();
        Task UpdateTechnologyAsync(IdNameDTO technology, string userName);
    }
}
