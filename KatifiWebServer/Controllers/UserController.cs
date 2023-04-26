using AutoMapper;
using KatifiWebServer.Models.DatabaseModels;
using KatifiWebServer.Models.DTOModels;
using KatifiWebServer.Services;
using Microsoft.AspNetCore.Mvc;

namespace KatifiWebServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly IMapper _mapper;

        public UserController(IUserService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            var users = await _service.GetAllAsync(u => u.Address);
            var userdtos = _mapper.Map<IEnumerable<UserDTO>>(users);

            return Ok(userdtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUser(int id)
        {
            var user = await _service.GetByIdAsync(id, u => u.Address);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<UserDTO>(user));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UserDTO>> PutUser(int id, UserDTO userdto)
        {
            var user = _mapper.Map<AppUser>(userdto);
            if (id != user.Id || !await _service.MeetsTheConstraints(user))
            {
                return BadRequest();
            }

            await _service.UpdateAsync(user);
            return Ok(_mapper.Map<UserDTO>(user));
        }

        [HttpPost]
        public async Task<ActionResult<UserDTO>> PostUser(UserDTO userdto)
        {
            var user = _mapper.Map<AppUser>(userdto);
            if (!await _service.MeetsTheConstraints(user))
            {
                return BadRequest();
            }

            await _service.AddAsync(user);
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, _mapper.Map<UserDTO>(user));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (!await _service.EntityExists(id))
            {
                return NotFound();
            }

            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
