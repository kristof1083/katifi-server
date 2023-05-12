using KatifiWebServer.Data.Base;
using KatifiWebServer.Models.DatabaseModels;

namespace KatifiWebServer.Services
{
    public interface IMessService : IEntityBaseRepository<Mess>
    {
        public Task<IEnumerable<Mess>> GetMessesFromChurch(int churchId);
    }
}
