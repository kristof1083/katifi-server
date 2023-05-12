using KatifiWebServer.Data;
using KatifiWebServer.Data.Base;
using KatifiWebServer.Models.DatabaseModels;
using Microsoft.EntityFrameworkCore;

namespace KatifiWebServer.Services
{
    public class MessService : EntityBaseRepository<Mess>, IMessService
    {
        private readonly MicrosoftEFContext _context;

        public MessService(MicrosoftEFContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Mess>> GetMessesFromChurch(int churchId)
        {
            return await this._context.Messes.Where(m => m.ChurchId == churchId).ToListAsync();
        }

        public override bool MeetsTheConstraints(Mess mess)
        {
            bool basebool = base.MeetsTheConstraints(mess);
            if (basebool && !string.IsNullOrWhiteSpace(mess.Day))
            {
                return true;
            }
            return false;
        }
    }
}
