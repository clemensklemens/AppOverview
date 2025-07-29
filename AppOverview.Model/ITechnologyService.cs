namespace AppOverview.Model
{
    public interface ITechnologyService
    {
        Task<TechnologyDTO> AddTechnologyAsync(TechnologyDTO technology);
        Task<IEnumerable<TechnologyDTO>> GetTechnologiesAsync();
        Task UpdateTechnologyAsync(TechnologyDTO technology);
    }
}
