using KatifiWebServer.Data;
using KatifiWebServer.Data.Base;
using KatifiWebServer.Models.DatabaseModels;

namespace KatifiWebServer.Services
{
    public class EventService : EntityBaseRepository<Event>, IEventService
    {
        public EventService(MicrosoftEFContext context) : base(context)
        {
        }

        public override async Task<bool> MeetsTheConstraints(Event eve)
        {
            bool basebool = await base.MeetsTheConstraints(eve);
            if (basebool && !string.IsNullOrWhiteSpace(eve.Name) && !string.IsNullOrWhiteSpace(eve.Organizer) &&
                eve.Date != DateTime.MinValue && eve.RegistrationDeadline != DateTime.MinValue && eve.Date >= eve.RegistrationDeadline)
            {
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }
    }
}
