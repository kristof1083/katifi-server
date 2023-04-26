using KatifiWebServer.Data;
using KatifiWebServer.Data.Base;
using KatifiWebServer.Models.DatabaseModels;

namespace KatifiWebServer.Services
{
    public class UserService : EntityBaseRepository<AppUser>, IUserService
    {
        public UserService(MicrosoftEFContext context) : base(context)
        {
        }

        public override async Task<bool> MeetsTheConstraints(AppUser user)
        {
            bool basebool = await base.MeetsTheConstraints(user);
            if (basebool && user.AgreeTerm == true && user.UserName != null && user.PasswordHash != null && user.FullName != null
                 && user.BornDatetime != DateTime.MinValue && (user.Gender == 'F' || user.Gender == 'M'))
            {
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }
    }
}
