using KatifiWebServer.Data;
using KatifiWebServer.Data.Base;
using KatifiWebServer.Models.DatabaseModels;
using Microsoft.EntityFrameworkCore;

namespace KatifiWebServer.Services
{
    public class ParticipantService : EntityBaseRepository<Participant>, IParticipantService
    {
        private readonly MicrosoftEFContext _context;

        public ParticipantService(MicrosoftEFContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Participant?> GetByIdsAsync(int eventId, int userId)
        {
            var q = _context.Participants.Include(p => p.Event).Include(p => p.User).Where(p => p.EventId == eventId && p.UserId == userId);
            return await q.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<int>> GetEventIdsByUserID(int userId)
        {
            return await _context.Participants.AsNoTracking().Where(p => p.UserId == userId).Select(p=>p.EventId).ToListAsync();
        }

        public async Task<IEnumerable<Participant>> GetParticipantsAsync(int eventId)
        {
            return await _context.Participants.Include(p => p.Event).Include(p => p.User).ThenInclude(u => u.Address).Where(p => p.EventId == eventId).ToListAsync();
        }

        public async Task<bool> UserIsRegistred(int eventId, int userId)
        {
            int partid = await _context.Participants.AsNoTracking().Where(p => p.EventId == eventId && p.UserId == userId).Select(p => p.Id).FirstOrDefaultAsync();
            if (partid > 0)
                return true;

            return false;
        }
    }
}
