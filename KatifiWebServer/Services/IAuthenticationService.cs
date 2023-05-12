using KatifiWebServer.Models.SecurityModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace KatifiWebServer.Services
{
    public interface IAuthenticationService
    {
        public Task<string?> Login(LoginModel model);

        public Task<int> RegistUserAsync(RegisterModel model);

        public Task<int> RegistAdminAsync(RegisterModel model);

        public Task<int> AddRoleToUserAsync(string roleName, string userName);
    }
}
