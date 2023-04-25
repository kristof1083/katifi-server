using Microsoft.AspNetCore.Mvc;
using KatifiWebServer.Models.DatabaseModels;
using AutoMapper;
using KatifiWebServer.Models.DTOModels;
using KatifiWebServer.Services;

namespace KatifiWebServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _service;
        private readonly IMapper _mapper;

        public AddressController(IMapper mapper, IAddressService service)
        {
            _mapper = mapper;
            _service = service;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<AddressDTO>>> GetAddresses()
        {
            var addresses = await _service.GetAllAsync();
            var addressdtos = _mapper.Map<IEnumerable<AddressDTO>>(addresses);

            return Ok(addressdtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AddressDTO>> GetAddress(int id)
        {
            var address = await _service.GetByIdAsync(id);

            if (address == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<AddressDTO>(address));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<AddressDTO>> PutAddress(int id, AddressDTO addressdto)
        {
            var address = _mapper.Map<Address>(addressdto);
            if (id != address.Id || !await _service.MeetsTheConstraints(address))
            {
                return BadRequest();
            }

            await _service.UpdateAsync(address);
            return Ok(_mapper.Map<AddressDTO>(address));
        }

        [HttpPost]
        public async Task<ActionResult<AddressDTO>> PostAddress(AddressDTO addressdto)
        {
            var address = _mapper.Map<Address>(addressdto);
            if(!await _service.MeetsTheConstraints(address))
            {
                return BadRequest();
            }

            await _service.AddAsync(address);
            return CreatedAtAction(nameof(GetAddress), new { id = address.Id }, _mapper.Map<AddressDTO>(address));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAddress(int id)
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
