using AutoMapper;
using KatifiWebServer.Models.DatabaseModels;
using KatifiWebServer.Models.DTOModels;
using KatifiWebServer.Services;
using Microsoft.AspNetCore.Mvc;

namespace KatifiWebServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommunityController : ControllerBase
    {
        private readonly ICommunityService _service;
        private readonly IMapper _mapper;

        public CommunityController(ICommunityService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommunityDTO>>> GetCommunityes()
        {
            var communityes = await _service.GetAllCommunityAsync();
            var communitydtos = _mapper.Map<IEnumerable<CommunityDTO>>(communityes);

            return Ok(communitydtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CommunityDTO>> GetCommunity(int id)
        {
            var community = await _service.GetByIdAsync(id); //id, c => c.Address, c => c.Members

            if (community == null)
            {
                return NotFound();
            }

            return Ok(community);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CommunityDTO>> PutCommunity(int id, CommunityDTO communitydto)
        {
            var community = _mapper.Map<Community>(communitydto);
            if (id != community.Id || !await _service.MeetsTheConstraints(community))
            {
                return BadRequest();
            }

            await _service.UpdateAsync(community);
            return Ok(_mapper.Map<CommunityDTO>(community));
        }

        [HttpPost]
        public async Task<ActionResult<CommunityDTO>> PostCommunity(CommunityDTO communitydto)
        {
            var community = _mapper.Map<Community>(communitydto);
            if (!await _service.MeetsTheConstraints(community))
            {
                return BadRequest();
            }

            await _service.AddAsync(community);
            return CreatedAtAction(nameof(GetCommunity), new { id = community.Id }, _mapper.Map<CommunityDTO>(community));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCommunity(int id)
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
