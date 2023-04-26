using AutoMapper;
using KatifiWebServer.Data.Enums;
using KatifiWebServer.Models.DatabaseModels;
using KatifiWebServer.Models.SecurityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace KatifiWebServer.Services
{
    public class AuthenticationService: IAuthenticationService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AuthenticationService(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IConfiguration configuration, IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _mapper = mapper;
        }

        public JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

        public async Task<object> Login(LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var token = GetToken(authClaims);

                return new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                };
            }
            return null;
        }

        public async Task<int> RegisterUser(RegisterModel model)
        {
            if (!HasValidProperties(model))
                return -1;

            var userExists = await _userManager.FindByNameAsync(model.UserName);
            if (userExists != null)
                return -2;

            AppUser user = _mapper.Map<AppUser>(model);
            user.SecurityStamp = Guid.NewGuid().ToString();
            user.RegistrationDate = DateTime.Now;

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return -3;

            if (!await _roleManager.RoleExistsAsync(AppRoleEnum.User.ToString()))
            {
                await _roleManager.CreateAsync(new AppRole { Name = AppRoleEnum.User.ToString() });
            }
            await _userManager.AddToRoleAsync(user, AppRoleEnum.User.ToString());

            return 0;
        }

        public async Task<int> RegisterAdmin(RegisterModel model)
        {
            if (!HasValidProperties(model))
                return -1;

            var userExists = await _userManager.FindByNameAsync(model.UserName);
            if (userExists != null)
                return -2;

            AppUser user = _mapper.Map<AppUser>(model);
            user.SecurityStamp = Guid.NewGuid().ToString();
            user.RegistrationDate = DateTime.Now;

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return -3;

            if (!await _roleManager.RoleExistsAsync(AppRoleEnum.Admin.ToString()))
                await _roleManager.CreateAsync(new AppRole { Name = AppRoleEnum.Admin.ToString() });
            if (!await _roleManager.RoleExistsAsync(AppRoleEnum.User.ToString()))
                await _roleManager.CreateAsync(new AppRole{ Name = AppRoleEnum.User.ToString() });

            await _userManager.AddToRoleAsync(user, AppRoleEnum.Admin.ToString());
            await _userManager.AddToRoleAsync(user, AppRoleEnum.User.ToString());
            
            return 0;
        }

        private bool HasValidProperties(RegisterModel user)
        {
            if(user != null)
            {
                if (user.AgreeTerm == true && !string.IsNullOrWhiteSpace(user.UserName) && !string.IsNullOrWhiteSpace(user.Password) && !string.IsNullOrWhiteSpace(user.FirstName)
                    && !string.IsNullOrWhiteSpace(user.Lastname) && (user.Gender == 'F' || user.Gender == 'M'))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
