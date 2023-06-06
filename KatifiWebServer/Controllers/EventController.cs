using AutoMapper;
using KatifiWebServer.Models.DatabaseModels;
using KatifiWebServer.Models.DTOModels;
using KatifiWebServer.Models.FilterModels;
using KatifiWebServer.Models.SecurityModels;
using KatifiWebServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KatifiWebServer.Controllers
{
    [Authorize(Roles = "EventOrganizer")]
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IParticipantService _participantservice;
        private readonly IAddressService _addressService;
        private readonly IImageFileService _fileService;
        private readonly IEventService _eventservice;
        private readonly IMapper _mapper;

        public EventController(IEventService eventservice, IParticipantService participantservice, IAddressService addressService,
             IImageFileService fileService, IMapper mapper)
        {
            _participantservice = participantservice;
            _addressService = addressService;
            _fileService = fileService;
            _eventservice = eventservice;
            _mapper = mapper;
        }

        #region Basic CRUD functions
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<EventDTO>>> GetEventes()
        {
            var events = await _eventservice.GetAllAsync(e => e.Address, e => e.Participants);
            var eventdtos = _mapper.Map<IEnumerable<EventDTO>>(events);

            return Ok(eventdtos);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<EventDTO>> GetEvent(int id)
        {
            var varevent = await _eventservice.GetByIdAsync(id, e => e.Address, e => e.Participants);

            if (varevent == null)
            {
                return NotFound();
            }

            var eventdto = _mapper.Map<EventDTO>(varevent);
            return Ok(eventdto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<EventDTO>> ModifyEvent(int id, EventDTO eventdto)
        {
            var varevent = _mapper.Map<Event>(eventdto);

            if (id != varevent.Id || !await _eventservice.EntityExists(id) || !_eventservice.MeetsTheConstraints(varevent))
                return BadRequest("Event fields are not correct.");

            if (!_addressService.MeetsTheConstraints(varevent.Address))
                return BadRequest("Address fields are not correct.");

            varevent.AddressId = await _addressService.HandleInternalAddressUpdate(varevent.Address);
            varevent.Address = await _addressService.GetByIdAsync(varevent.AddressId);

            await _eventservice.UpdateAsync(varevent);
            await _addressService.DeleteUnusedAddresses();
            return Ok(_mapper.Map<EventDTO>(varevent));
        }

        [HttpPost]
        public async Task<ActionResult<EventDTO>> AddEvent(EventDTO eventdto)
        {
            var varevent = _mapper.Map<Event>(eventdto);

            if (!_eventservice.MeetsTheConstraints(varevent))
                return BadRequest("Event missing mandatory fields.");

            if (!_addressService.MeetsTheConstraints(varevent.Address))
                return BadRequest("Address fields are not correct.");

            varevent.AddressId = await _addressService.HandleInternalAddressUpdate(varevent.Address);
            varevent.Address = await _addressService.GetByIdAsync(varevent.AddressId);

            await _eventservice.AddAsync(varevent);
            return CreatedAtAction(nameof(GetEvent), new { id = varevent.Id }, _mapper.Map<EventDTO>(varevent));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            if (!await _eventservice.EntityExists(id))
            {
                return NotFound();
            }

            await _eventservice.DeleteAsync(id);
            await _addressService.DeleteUnusedAddresses();
            return NoContent();
        }
        #endregion


        [HttpGet("filter")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<EventDTO>>> GetFilteredEvents(
            string? countryCode, string? city, bool active, string? eventname, int minparticipant, DateTime? earliestdate, DateTime? latestdate)
        {
            var filter = new EventFilter
            {
                CountryCode = countryCode,
                City = city,
                JustActives = active,
                EventNameSubstring = eventname,
                MinimumParticipant = minparticipant,
                EarliestEventDate = earliestdate,
                LatestEventDate = latestdate
            };

            var events = await _eventservice.GetFilteredEventsAsync(filter);
            if (!events.Any())
                return NotFound();

            var eventdtos = _mapper.Map<IEnumerable<EventDTO>>(events);
            return Ok(eventdtos);
        }


        #region Events and Users
        [HttpGet("{id}/participants")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<IEnumerable<ParticipantDTO>>> GetEventParticipants(int id)
        {
            if (!await _eventservice.EntityExists(id))
                return NotFound();

            var participants = await _participantservice.GetParticipantsAsync(id);

            if (!participants.Any())
                return NotFound();

            var participantdos = _mapper.Map<IEnumerable<ParticipantDTO>>(participants);

            return Ok(participantdos);
        }

        [HttpGet("my-events")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<IEnumerable<EventDTO>>> GetUsersEvents(int userId)
        {
            var eventIds = await _participantservice.GetEventIdsByUserID(userId);

            if (!eventIds.Any())
                return NotFound();

            List<Event> events = new();

            foreach (var id in eventIds)
                events.Add(await _eventservice.GetByIdAsync(id, e => e.Address, e => e.Participants));

            return Ok(_mapper.Map<IEnumerable<EventDTO>>(events));
        }

        [HttpPost("{eventId}/regist")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> RegistUserForEvent(int eventId, [FromBody] int userId)
        {
            if (!await _eventservice.EntityExists(eventId))
                return BadRequest("Event or User does not exist.");

            if (await _participantservice.UserIsRegistred(eventId, userId))
                return Conflict("User is already registradted to event.");

            var actevent =  await _eventservice.GetByIdAsync(eventId);

            if (DateTime.Now >= actevent.Start)
                return BadRequest("The event has started. Can not accept application.");

            if (actevent.MaxParticipant is not null)
            {
                if (actevent.Participants.Count >= actevent.MaxParticipant)
                    return BadRequest("Event is full. Registration denied.");
            }

            var participant = new Participant()
            {
                EventId = eventId,
                UserId = userId,
                ApplicationDate = DateTime.Now
            };

            await _participantservice.AddAsync(participant);
            return Ok("Successfully registred."); ;
        }

        [HttpDelete("{eventId}/cancel-registration")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> DeleteRegistration(int eventId, int userId)
        {
            var participant = await _participantservice.GetByIdsAsync(eventId, userId);

            if (participant == null)
                return NotFound(new Response { Status = "Error", Message = string.Format("User with id {0} is not registred to event {1}.", userId, eventId) });

            if (DateTime.Now >= participant.Event.Start)
                return BadRequest("The event has started. Can not accept application canceling.");

            await _participantservice.DeleteAsync(participant.Id);
            return NoContent();
        }

        [HttpPost("{eventId}/upload-photo")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> UploadPhotos(int eventId,[FromForm] int userId, [FromForm] IFormFile file)
        {
            if (!await _participantservice.UserIsRegistred(eventId, userId))
                return BadRequest("Sorry, you were not registered for this event.");

            /* MULTI FILE CODE
            if (files == null || files.Count() > 20)
                return BadRequest("Sorry, you are not allowed to upload more than 20 photos.");
            */
            var eventDate = (await _eventservice.GetByIdAsync(eventId)).End;
            if(eventDate > DateTime.Now)
                return BadRequest("Sorry, you can only upload photos after the event.");

            long succesSize = 0;
            int successNumber = 0;
            string eventName = (await _eventservice.GetByIdAsync(eventId)).Name;

            var succcess = await _fileService.SaveImageAsync(file, $"EVENT_{eventId}_{eventName}", $"IMG_{userId}");
            if (succcess)
            {
                succesSize += file.Length;
                successNumber++;
            }
            /* - MULTI FILE CODE
            foreach (var formFile in files)
            {
                var succcess = await _fileService.SaveImageAsync(formFile, $"EVENT_{eventId}_{eventName}", $"IMG_{userId}");
                if (succcess)
                {
                    succesSize += formFile.Length;
                    successNumber++;
                }
            }*/

            if (succesSize == 0)
                return BadRequest("Can not process any given file.");
                
            return Ok($"{successNumber} file was succesfully uploaded. Total size: {succesSize} byte. ");
        }


        [HttpGet("{eventId}/photos")]
        [Authorize(Roles = "User")]
        public IActionResult GetEventPhotos(int eventId, int userId)
        {
            return Forbid();
        }

        #endregion
    }
}
