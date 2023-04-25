using AutoMapper;
using KatifiWebServer.Models.DatabaseModels;
using KatifiWebServer.Models.DTOModels;
using KatifiWebServer.Services;
using Microsoft.AspNetCore.Mvc;

namespace KatifiWebServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventService _service;
        private readonly IMapper _mapper;

        public EventController(IEventService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventDTO>>> GetEventes()
        {
            var events = await _service.GetAllAsync();
            var eventdtos = _mapper.Map<IEnumerable<EventDTO>>(events);

            return Ok(eventdtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EventDTO>> GetEvent(int id)
        {
            var varevent = await _service.GetByIdAsync(id);

            if (varevent == null)
            {
                return NotFound();
            }

            return Ok(varevent);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<EventDTO>> PutEvent(int id, EventDTO eventdto)
        {
            var varevent = _mapper.Map<Event>(eventdto);
            if (id != varevent.Id || !await _service.MeetsTheConstraints(varevent))
            {
                return BadRequest();
            }

            await _service.UpdateAsync(varevent);
            return Ok(_mapper.Map<EventDTO>(varevent));
        }

        [HttpPost]
        public async Task<ActionResult<EventDTO>> PostEvent(EventDTO eventdto)
        {
            var varevent = _mapper.Map<Event>(eventdto);
            if (!await _service.MeetsTheConstraints(varevent))
            {
                return BadRequest();
            }

            await _service.AddAsync(varevent);
            return CreatedAtAction(nameof(GetEvent), new { id = varevent.Id }, _mapper.Map<EventDTO>(varevent));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
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
