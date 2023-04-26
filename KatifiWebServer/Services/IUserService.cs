using KatifiWebServer.Data.Base;
using KatifiWebServer.Models.DatabaseModels;

namespace KatifiWebServer.Services
{
    public interface IUserService : IEntityBaseRepository<AppUser>
    {
    }
}
