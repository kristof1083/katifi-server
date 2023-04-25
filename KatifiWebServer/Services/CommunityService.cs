using KatifiWebServer.Data;
using KatifiWebServer.Data.Base;
using KatifiWebServer.Models.DatabaseModels;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace KatifiWebServer.Services
{
    public class CommunityService : EntityBaseRepository<Community>, ICommunityService
    {
        private readonly MicrosoftEFContext _context;

        public CommunityService(MicrosoftEFContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<bool> MeetsTheConstraints(Community com)
        {
            bool basebool = await base.MeetsTheConstraints(com);
            if (basebool && !string.IsNullOrWhiteSpace(com.Name) && com.AddressId >= 338)
            {
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<IEnumerable<Community>> GetAllCommunityAsync()
        {
            var q = _context.Communities.Include(c => c.Address).Include(c=>c.Members).ThenInclude(m=>m.User);
            return await q.ToListAsync();
        }
    }
}
