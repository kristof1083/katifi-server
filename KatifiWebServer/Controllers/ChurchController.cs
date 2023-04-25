using AutoMapper;
using KatifiWebServer.Models.DatabaseModels;
using KatifiWebServer.Models.DTOModels;
using KatifiWebServer.Services;
using Microsoft.AspNetCore.Mvc;


namespace KatifiWebServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChurchController : ControllerBase
    {
        private readonly IChurchService _service;
        private readonly IMapper _mapper;

        public ChurchController(IChurchService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChurchDTO>>> GetChurches()
        {
            var churches = await _service.GetAllAsync(c => c.Address, c => c.Messes);
            var churchdtos = _mapper.Map<IEnumerable<ChurchDTO>>(churches);

            return Ok(churchdtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ChurchDTO>> GetChurch(int id)
        {
            var church = await _service.GetByIdAsync(id, c => c.Address, c => c.Messes);

            if (church == null)
            {
                return NotFound();
            }

            return Ok(church);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ChurchDTO>> PutChurch(int id, ChurchDTO churchdto)
        {
            var church = _mapper.Map<Church>(churchdto);
            if (id != church.Id || !await _service.MeetsTheConstraints(church))
            {
                return BadRequest();
            }

            await _service.UpdateAsync(church);
            return Ok(_mapper.Map<ChurchDTO>(church));
        }

        [HttpPost]
        public async Task<ActionResult<ChurchDTO>> PostChurch(ChurchDTO churchdto)
        {
            var church = _mapper.Map<Church>(churchdto);
            if (!await _service.MeetsTheConstraints(church))
            {
                return BadRequest();
            }

            await _service.AddAsync(church);
            return CreatedAtAction(nameof(GetChurch), new { id = church.Id }, _mapper.Map<ChurchDTO>(church));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChurch(int id)
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
