using KatifiWebServer.Data;
using KatifiWebServer.Data.Base;
using KatifiWebServer.Models.DatabaseModels;
using Microsoft.EntityFrameworkCore;

namespace KatifiWebServer.Services
{
    public class ChurchService : EntityBaseRepository<Church>, IChurchService
    {
        private readonly MicrosoftEFContext _context;

        public ChurchService(MicrosoftEFContext context) : base(context)
        {
            _context = context;
        }

        public override bool MeetsTheConstraints(Church church)
        {
            bool basebool = base.MeetsTheConstraints(church);

            if (basebool && !string.IsNullOrWhiteSpace(church.Name)) {
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<Church>> GetChurchesInCity(string cityName)
        {
            return await this._context.Churches.Include(c => c.Address).Where(c => c.Address.City.ToLower() == cityName.ToLower()).ToListAsync();
        }
    }
}
