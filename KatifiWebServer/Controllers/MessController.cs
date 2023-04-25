using AutoMapper;
using KatifiWebServer.Models.DatabaseModels;
using KatifiWebServer.Models.DTOModels;
using KatifiWebServer.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KatifiWebServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessController : ControllerBase
    {
        private readonly IMessService _service;
        private readonly IMapper _mapper;

        public MessController(IMessService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessDTO>>> GetMesses()
        {
            var messes = await _service.GetAllAsync();
            var messdtos = _mapper.Map<IEnumerable<MessDTO>>(messes);

            return Ok(messdtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MessDTO>> GetMess(int id)
        {
            var mess = await _service.GetByIdAsync(id);

            if (mess == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<MessDTO>(mess));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<MessDTO>> PutMess(int id, MessDTO messdto)
        {
            var mess = _mapper.Map<Mess>(messdto);
            if (id != mess.Id || !await _service.MeetsTheConstraints(mess))
            {
                return BadRequest();
            }

            await _service.UpdateAsync(mess);
            return Ok(_mapper.Map<MessDTO>(mess));
        }

        [HttpPost]
        public async Task<ActionResult<MessDTO>> PostMess(MessDTO messdto)
        {
            var mess = _mapper.Map<Mess>(messdto);
            if (!await _service.MeetsTheConstraints(mess))
            {
                return BadRequest();
            }

            await _service.AddAsync(mess);
            return CreatedAtAction(nameof(GetMess), new { id = mess.Id }, _mapper.Map<MessDTO>(mess));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMess(int id)
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
