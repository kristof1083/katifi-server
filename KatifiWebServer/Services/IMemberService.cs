using KatifiWebServer.Data.Base;
using KatifiWebServer.Models.DatabaseModels;

namespace KatifiWebServer.Services
{
    public interface IMemberService : IEntityBaseRepository<Member>
    {
        public Task<Member?> GetByIdsAsync(int communityId, int userId);
        public Task<IEnumerable<Member>> GetMembersAsync(int communityId, bool justactiveones = false);
    }
}
