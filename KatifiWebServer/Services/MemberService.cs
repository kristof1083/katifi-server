using KatifiWebServer.Data;
using KatifiWebServer.Data.Base;
using KatifiWebServer.Models.DatabaseModels;
using Microsoft.EntityFrameworkCore;

namespace KatifiWebServer.Services
{
    public class MemberService : EntityBaseRepository<Member>, IMemberService
    {
        private readonly MicrosoftEFContext _context;

        public MemberService(MicrosoftEFContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Member?> GetByIdsAsync(int communityId, int userId)
        {
            var q = _context.Members.Include(m => m.Community).Include(m => m.User).Where(m => m.CommunityId == communityId && m.UserId == userId);
            return await q.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Member>> GetMembersAsync(int communityId, bool justactiveones)
        {
            var q = _context.Members.Include(m => m.Community).Include(m => m.User).ThenInclude(u=>u.Address).Where(m=>m.CommunityId == communityId);

            if (justactiveones)
            {
                q = q.Where(m => m.LeaveDate == null);
            }

            return await q.ToListAsync();
        }
    }
}
