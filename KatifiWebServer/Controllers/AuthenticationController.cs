using KatifiWebServer.Models.DatabaseModels;
using KatifiWebServer.Models.SecurityModels;
using KatifiWebServer.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KatifiWebServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService service)
        {
            _authenticationService = service;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var tokeninfo = await _authenticationService.Login(model);
            if (tokeninfo == null)
            {
                return Unauthorized();
            }
            return Ok(tokeninfo);
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            int status = await _authenticationService.RegisterUser(model);
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
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            int status = await _authenticationService.RegisterAdmin(model);
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

    }
}
