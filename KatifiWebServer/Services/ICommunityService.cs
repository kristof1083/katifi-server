using KatifiWebServer.Data.Base;
using KatifiWebServer.Models.DatabaseModels;
using KatifiWebServer.Models.FilterModels;

namespace KatifiWebServer.Services
{
    public interface ICommunityService : IEntityBaseRepository<Community>
    {
        public Task<IEnumerable<Community>> GetAllCommunityAsync();

        public Task<IEnumerable<Community>> GetFilteredCommunitiesAsync(CommunityFilter filter);
    }
}
