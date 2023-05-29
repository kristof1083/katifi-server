using KatifiWebServer.Models.DatabaseModels;
using KatifiWebServer.Models.SecurityModels;

namespace KatifiWebServer.Services
{
    public interface IAuthenticationService
    {
        public Task<string?> Login(LoginModel model);

        public Task<AppUser?> GetUserAsync(LoginModel model);

        public Task<int> RegistUserAsync(RegisterModel model);

        public Task<int> RegistAdminAsync(RegisterModel model);

        public Task<int> AddRoleToUserAsync(string roleName, string userName);
    }
}
