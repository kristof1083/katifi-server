using KatifiWebServer.Data.Base;
using KatifiWebServer.Models.DatabaseModels;
using KatifiWebServer.Models.FilterModels;

namespace KatifiWebServer.Services
{
    public interface IEventService : IEntityBaseRepository<Event>
    {
        public Task<IEnumerable<Event>> GetAllEventAsync();
        public Task<IEnumerable<Event>> GetFilteredEventsAsync(EventFilter filter);
    }
}
