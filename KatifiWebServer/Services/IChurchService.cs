using KatifiWebServer.Data.Base;
using KatifiWebServer.Models.DatabaseModels;

namespace KatifiWebServer.Services
{
    public interface IChurchService : IEntityBaseRepository<Church>
    {
        public Task<IEnumerable<Church>> GetChurchesInCity(string cityName);
    }
}
