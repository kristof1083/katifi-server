using KatifiWebServer.Data;
using KatifiWebServer.Data.Base;
using KatifiWebServer.Models.DatabaseModels;
using KatifiWebServer.Models.DTOModels;

namespace KatifiWebServer.Services
{
    public class UserService : EntityBaseRepository<AppUser>, IUserService
    {
        public UserService(MicrosoftEFContext context) : base(context)
        {
        }

        public override bool MeetsTheConstraints(AppUser user)
        {
            bool basebool = base.MeetsTheConstraints(user);
            if (basebool && user.AgreeTerm == true && user.UserName != null && user.PasswordHash != null && user.FullName != null
                 && user.BornDatetime != DateTime.MinValue && (user.Gender == 'F' || user.Gender == 'M'))
            {
                return true;
            }
            return false;
        }
    }
}
