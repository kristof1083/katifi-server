using AutoMapper;
using KatifiWebServer.Models.DatabaseModels;
using KatifiWebServer.Models.DTOModels;
using KatifiWebServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KatifiWebServer.Controllers
{
    [Authorize(Roles = "Vicar")]
    [Route("api/[controller]")]
    [ApiController]
    public class ChurchController : ControllerBase
    {
        private readonly IChurchService _churchService;
        private readonly IMessService _messService;
        private readonly IAddressService _addressService;
        private readonly IMapper _mapper;

        public ChurchController(IChurchService churchservice, IMessService messservice, IAddressService addressservice, IMapper mapper)
        {
            _churchService = churchservice;
            _messService = messservice;
            _addressService = addressservice;
            _mapper = mapper;
        }

        #region Basic CRUD Functions
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ChurchDTO>>> GetChurches()
        {
            var churches = await _churchService.GetAllAsync(c => c.Address);
            var churchdtos = _mapper.Map<IEnumerable<ChurchDTO>>(churches);

            return Ok(churchdtos);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ChurchDTO>> GetChurch(int id)
        {
            var church = await _churchService.GetByIdAsync(id, c => c.Address);

            if (church == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<ChurchDTO>(church));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ChurchDTO>> ModifyChurch(int id, ChurchDTO churchdto)
        {
            var church = _mapper.Map<Church>(churchdto);
            if (id != church.Id || !_churchService.MeetsTheConstraints(church))
                return BadRequest("Church fields are not correct.");

            if (!_addressService.MeetsTheConstraints(church.Address))
                return BadRequest("Address fields are not correct.");

            int newaddressid = await _addressService.HandleInternalAddressUpdate(church.Address);
            if (await _addressService.HasChurchInLocation(newaddressid))
                return BadRequest("There is a church at this location");

            church.AddressId = newaddressid;
            church.Address = await _addressService.GetByIdAsync(newaddressid);

            await _churchService.UpdateAsync(church);
            await _addressService.DeleteUnusedAddresses();
            return Ok(_mapper.Map<ChurchDTO>(church));
        }

        [HttpPost]
        public async Task<ActionResult<ChurchDTO>> AddChurch(ChurchDTO churchdto)
        {
            var church = _mapper.Map<Church>(churchdto);

            if (!_churchService.MeetsTheConstraints(church))
                return BadRequest("Church fields are not correct.");

            if (!_addressService.MeetsTheConstraints(church.Address))
                return BadRequest("Address fields are not correct.");

            int newaddressid = await _addressService.HandleInternalAddressUpdate(church.Address);
            if (await _addressService.HasChurchInLocation(newaddressid))
                return BadRequest("There is a church at this location");

            church.AddressId = newaddressid;
            church.Address = await _addressService.GetByIdAsync(newaddressid);

            await _churchService.AddAsync(church);
            return CreatedAtAction(nameof(GetChurch), new { id = church.Id }, _mapper.Map<ChurchDTO>(church));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChurch(int id)
        {
            if (!await _churchService.EntityExists(id))
            {
                return NotFound();
            }

            await _churchService.DeleteAsync(id);
            await _addressService.DeleteUnusedAddresses();
            return NoContent();
        }
        #endregion

        [HttpGet("cities/{cityName}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ChurchDTO>>> GetChurchesInCity(string cityName)
        {
            var churches = await _churchService.GetChurchesInCity(cityName);

            if (!churches.Any())
            {
                return NotFound(string.Format("No church in the city named {0}.", cityName));
            }
            var churcdtos = _mapper.Map<IEnumerable<ChurchDTO>>(churches);
            return Ok(churcdtos);
        }

        #region Mess functions
        [HttpGet("{id}/messes")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<MessDTO>>> GetMessesInChurch(int id)
        {
            bool exist = _churchService.EntityExists(id).Result;
            if (!exist)
            {
                return NotFound("Church is not exist with the given ID.");
            }

            var messes = await _messService.GetMessesFromChurch(id);
            var messdtos = _mapper.Map<IEnumerable<MessDTO>>(messes);
            return Ok(messdtos);
        }

        [HttpPost("{id}/messes/add-mess")]
        public async Task<IActionResult> AddMess(int id, MessDTO messdto)
        {
            if (id != messdto.ChurchId)
                return BadRequest("Ids not match.");

            if(!await _churchService.EntityExists(id))
                return NotFound("Church is not exist with the given ID.");

            var mess = _mapper.Map<Mess>(messdto);

            if (!_messService.MeetsTheConstraints(mess))
                return BadRequest("Mess does not contain one or more mandatory field.");

            await _messService.AddAsync(mess);
            return Ok();
        }

        [HttpDelete("{id}/messes/{messId}/delete-mess")]
        public async Task<IActionResult> DeleteMess(int id, int messId)
        {
            if (!await _messService.EntityExists(messId))
                return NotFound("Mess is not exist with the given ID.");

            var messes = await _messService.GetMessesFromChurch(id);
            var mess = messes.Where(m=>m.Id == messId).FirstOrDefault();
            if (mess == null)
                return BadRequest("No such messid in church.");


            await _messService.DeleteAsync(messId);
            return NoContent();
        }
        #endregion
    }
}
