using KatifiWebServer.Data.Base;
using KatifiWebServer.Models.DatabaseModels;

namespace KatifiWebServer.Services
{
    public interface ICommunityService : IEntityBaseRepository<Community>
    {
        public Task<IEnumerable<Community>> GetAllCommunityAsync();
    }
}
