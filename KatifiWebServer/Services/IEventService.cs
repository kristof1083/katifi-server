using KatifiWebServer.Data.Base;
using KatifiWebServer.Models.DatabaseModels;

namespace KatifiWebServer.Services
{
    public interface IEventService : IEntityBaseRepository<Event>
    {
    }
}
