namespace AppOverview.Model
{
    public interface ITechnologyService
    {
        Task<TechnologyDTO> AddTechnologyAsync(TechnologyDTO technology);
        Task<TechnologyDTO> GetTechnologyAsync(int id);
        Task<IEnumerable<TechnologyDTO>> GetAllTechnologiesAsync();
        Task UpdateTechnologyAsync(TechnologyDTO technology);
    }
}
