using KatifiWebServer.Data.Base;
using KatifiWebServer.Models.DatabaseModels;

namespace KatifiWebServer.Services
{
    public interface IParticipantService : IEntityBaseRepository<Participant>
    {
        public Task<Participant?> GetByIdsAsync(int eventId, int userId);
        public Task<IEnumerable<int>> GetEventIdsByUserID(int userId);
        public Task<IEnumerable<Participant>> GetParticipantsAsync(int eventId);
        public Task<bool> UserIsRegistred(int eventId, int userId);
    }
}
