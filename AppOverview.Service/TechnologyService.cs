using AppOverview.Model;

namespace AppOverview.Service
{
    public class TechnologyService : ITechnologyService
    {
        private readonly IDataProvider _dataProvider;

        public TechnologyService(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public async Task<TechnologyDTO> AddTechnologyAsync(TechnologyDTO technology)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TechnologyDTO>> GetAllTechnologiesAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<TechnologyDTO> GetTechnologyAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateTechnologyAsync(TechnologyDTO technology)
        {
            throw new NotImplementedException();
        }
    }
}
