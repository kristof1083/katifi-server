using KatifiWebServer.Models.SecurityModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace KatifiWebServer.Services
{
    public interface IAuthenticationService
    {
        public Task<object> Login(LoginModel model);

        public Task<int> RegisterUser(RegisterModel model);

        public Task<int> RegisterAdmin(RegisterModel model);

        public JwtSecurityToken GetToken(List<Claim> authClaims);
    }
}
