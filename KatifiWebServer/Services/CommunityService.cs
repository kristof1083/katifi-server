using KatifiWebServer.Data;
using KatifiWebServer.Data.Base;
using KatifiWebServer.Models.DatabaseModels;
using KatifiWebServer.Models.FilterModels;
using Microsoft.EntityFrameworkCore;

namespace KatifiWebServer.Services
{
    public class CommunityService : EntityBaseRepository<Community>, ICommunityService
    {
        private readonly MicrosoftEFContext _context;

        public CommunityService(MicrosoftEFContext context) : base(context)
        {
            _context = context;
        }

        public override bool MeetsTheConstraints(Community com)
        {
            bool basebool = base.MeetsTheConstraints(com);
            if (basebool && !string.IsNullOrWhiteSpace(com.Name))
            {
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<Community>> GetAllCommunityAsync()
        {
            var q = _context.Communities.Include(c => c.Address).Include(c=>c.Members).ThenInclude(m=>m.User);
            return await q.ToListAsync();
        }

        public async Task<IEnumerable<Community>> GetFilteredCommunitiesAsync(CommunityFilter filter)
        {
            IQueryable<Community> query = _context.Communities.Include(c => c.Address).Include(c => c.Members).ThenInclude(m => m.User);
            
            if (!string.IsNullOrWhiteSpace(filter.CountryCode))
            {
                query = query.Where(c => c.Address.CountryCode.ToLower() == filter.CountryCode.ToLower());
            }
            if (!string.IsNullOrWhiteSpace(filter.City))
            {
                query = query.Where(c => c.Address.City.ToLower() == filter.City.ToLower());
            }
            if (filter.JustOpens)
            {
                query = query.Where(c => c.IsOpen == true);
            }
            if (!string.IsNullOrWhiteSpace(filter.CommunityNameSubstring))
            {
                query = query.Where(c => c.Name.ToLower().Contains(filter.CommunityNameSubstring.ToLower()));
            }
            if (filter.MinimumMember > 0)
            {
                query = query.Where(c => c.Members.Count() >= filter.MinimumMember);
            }

            return await query.ToListAsync();
        }
    }
}
