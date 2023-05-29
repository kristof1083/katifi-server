using AutoMapper;
using KatifiWebServer.Models.DatabaseModels;
using KatifiWebServer.Models.DTOModels;
using KatifiWebServer.Models.SecurityModels;
using KatifiWebServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KatifiWebServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IAddressService _addressService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IAuthenticationService authservice, IUserService service, IAddressService addressService, IMapper mapper)
        {
            _authenticationService = authservice;
            _addressService = addressService;
            _userService = service;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<ActionResult<string>> Login([FromBody] LoginModel model)
        {
            var jwttoken = await _authenticationService.Login(model);
            if (jwttoken == null)
            {
                return Unauthorized();
            }
            var myUser = await _authenticationService.GetUserAsync(model);

            var loggedInModel = new LoggedInModel
            {
                JwtToken = jwttoken,
                LoggedInUser = _mapper.Map<UserDTO>(myUser)
            };

            return Ok(loggedInModel);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            var users = await _userService.GetAllAsync(u => u.Address);
            var userdtos = _mapper.Map<IEnumerable<UserDTO>>(users);

            return Ok(userdtos);
        }

        [HttpGet("me")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<UserDTO>> GetMyUser([FromBody] LoginModel model)
        {
            var user = await _authenticationService.GetUserAsync(model);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<UserDTO>(user));
        }

        #region Regist users with roles

        [HttpPost]
        [Route("regist")]
        [AllowAnonymous]
        public async Task<IActionResult> RegistUser([FromBody] RegisterModel model)
        {
            int status = await _authenticationService.RegistUserAsync(model);
            switch (status)
            {
                case -1:
                    return BadRequest(new Response { Status = "Error", Message = "User parameters are not correct. Please correct it/them and try again." });
                case -2:
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });
                case -3:
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });
                case 0:
                    return Ok(new Response { Status = "Success", Message = "User created successfully!" });
                default:
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Unexpected error! Try again later." });
            }
        }

        [HttpPost]
        [Route("register-admin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RegistAdmin([FromBody] RegisterModel model)
        {
            int status = await _authenticationService.RegistAdminAsync(model);
            switch (status)
            {
                case -1:
                    return BadRequest(new Response { Status = "Error", Message = "User parameters are not correct. Please correct it/them and try again." });
                case -2:
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });
                case -3:
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });
                case 0:
                    return Ok(new Response { Status = "Success", Message = "User created successfully!" });
                default:
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Unexpected error! Try again later." });
            }
        }

        [HttpPost]
        [Route("assign-role/{roleName}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignRoleToUser(string roleName, [FromBody] string userName)
        {
            int status = await _authenticationService.AddRoleToUserAsync(roleName, userName);
            switch (status)
            {
                case -1:
                    return BadRequest(new Response { Status = "Error", Message = "Role does not exist." });
                case -2:
                    return BadRequest(new Response { Status = "Error", Message = "User does not exist." });
                case -3:
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Server error", Message = "Role assigning not possible!" });
                default:
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Server error", Message = "Unexpected error! Try again later." });
            }
        }

        #endregion

        [HttpPut("{id}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<UserDTO>> PutUser(int id, UserDTO userdto)
        {
            var user = _mapper.Map<AppUser>(userdto);
            if (id != user.Id || !await _userService.EntityExists(id) || !_userService.MeetsTheConstraints(user))
                return BadRequest();

            if(user.Address != null && user.AddressId != null)
            {
                if (!_addressService.MeetsTheConstraints(user.Address))
                    return BadRequest("Address fields are not correct.");

                int newaddressid = await _addressService.HandleInternalAddressUpdate(user.Address);

                user.AddressId = newaddressid;
                user.Address = await _addressService.GetByIdAsync(newaddressid);
            }

            await _userService.UpdateAsync(user);
            await _addressService.DeleteUnusedAddresses();
            return Ok(_mapper.Map<UserDTO>(user));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (!await _userService.EntityExists(id))
            {
                return NotFound();
            }

            await _userService.DeleteAsync(id);
            await _addressService.DeleteUnusedAddresses();
            return NoContent();
        }

    }
}
