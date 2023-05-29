using KatifiWebServer.Data;
using KatifiWebServer.Data.Base;
using KatifiWebServer.Models.DatabaseModels;
using KatifiWebServer.Models.FilterModels;
using Microsoft.EntityFrameworkCore;

namespace KatifiWebServer.Services
{
    public class EventService : EntityBaseRepository<Event>, IEventService
    {
        private readonly MicrosoftEFContext _context;

        public EventService(MicrosoftEFContext context) : base(context)
        {
            _context = context;
        }

        public override bool MeetsTheConstraints(Event eve)
        {
            bool basebool = base.MeetsTheConstraints(eve);
            if (basebool && !string.IsNullOrWhiteSpace(eve.Name) && !string.IsNullOrWhiteSpace(eve.Organizer) &&
                eve.Start != DateTime.MinValue && eve.RegistrationDeadline != DateTime.MinValue && eve.Start >= eve.RegistrationDeadline && eve.Start <= eve.End)
            {
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<Event>> GetAllEventAsync()
        {
            var q = _context.Events.Include(e=>e.Address).Include(e => e.Participants).ThenInclude(p => p.User);
            return await q.ToListAsync();
        }

        public async Task<IEnumerable<Event>> GetFilteredEventsAsync(EventFilter filter)
        {
            IQueryable<Event> query = _context.Events.Include(e => e.Address).Include(e => e.Participants).ThenInclude(p => p.User);

            if(filter.EarliestEventDate == null)
                filter.EarliestEventDate = DateTime.MinValue;

            if (filter.LatestEventDate == null || filter.LatestEventDate == DateTime.MinValue)
                filter.LatestEventDate = DateTime.MaxValue;

            query = query.Where(e => e.Start.Date >= filter.EarliestEventDate.Value.Date && e.End.Date <= filter.LatestEventDate.Value.Date);


            if (!string.IsNullOrWhiteSpace(filter.CountryCode))
            {
                query = query.Where(e => e.Address.CountryCode.ToLower() == filter.CountryCode.ToLower());
            }
            if (!string.IsNullOrWhiteSpace(filter.City))
            {
                query = query.Where(e => e.Address.City.ToLower() == filter.City.ToLower());
            }
            if (!string.IsNullOrWhiteSpace(filter.EventNameSubstring))
            {
                query = query.Where(e => e.Name.ToLower().Contains(filter.EventNameSubstring.ToLower()));
            }
            if (filter.JustActives)
            {
                query = query.Where(e => e.End.Date >= DateTime.Now.Date);
            }
            if (filter.MinimumParticipant > 0)
            {
                query = query.Where(e => e.Participants.Count() >= filter.MinimumParticipant);
            }

            return await query.ToListAsync();
        }


    }
}
