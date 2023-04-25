using KatifiWebServer.Data;
using KatifiWebServer.Data.Base;
using KatifiWebServer.Models.DatabaseModels;

namespace KatifiWebServer.Services
{
    public class UserService : EntityBaseRepository<User>, IUserService
    {
        public UserService(MicrosoftEFContext context) : base(context)
        {
        }

        public override async Task<bool> MeetsTheConstraints(User user)
        {
            bool basebool = await base.MeetsTheConstraints(user);
            if (basebool && user.AgreeTerm == true && user.Username != null && user.Password != null && user.FullName != null
                 && user.BornDatetime != DateTime.MinValue && user.RoleId >=20 && (user.Gender == 'F' || user.Gender == 'M'))
            {
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }
    }
}
