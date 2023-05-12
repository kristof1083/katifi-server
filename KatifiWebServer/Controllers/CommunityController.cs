using AutoMapper;
using KatifiWebServer.Data.Enums;
using KatifiWebServer.Models.DatabaseModels;
using KatifiWebServer.Models.DTOModels;
using KatifiWebServer.Models.FilterModels;
using KatifiWebServer.Models.SecurityModels;
using KatifiWebServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KatifiWebServer.Controllers
{
    [Authorize(Roles = "CommunityLeader")]
    [Route("api/[controller]")]
    [ApiController]
    public class CommunityController : ControllerBase
    {
        private readonly ICommunityService _communityService;
        private readonly IAddressService _addressService;
        private readonly IMemberService _memberService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;


        public CommunityController(ICommunityService communityService,IMemberService memberService, IAddressService addressService, IUserService userService, IMapper mapper)
        {
            _communityService = communityService;
            _addressService = addressService;
            _memberService = memberService;
            _userService = userService;
            _mapper = mapper;
        }

        #region Basic CRUD Functions
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<CommunityDTO>>> GetCommunityes()
        {
            var communityes = await _communityService.GetAllAsync(c => c.Address, c => c.Members);
            var communitydtos = _mapper.Map<IEnumerable<CommunityDTO>>(communityes);

            return Ok(communitydtos);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<CommunityDTO>> GetCommunity(int id)
        {
            var community = await _communityService.GetByIdAsync(id, c => c.Address, c => c.Members);

            if (community == null)
            {
                return NotFound();
            }

            var commdto = _mapper.Map<CommunityDTO>(community);
            return Ok(commdto);
        }

        [HttpPut("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<CommunityDTO>> ModifyCommunity(int id, CommunityDTO communitydto)
        {
            var community = _mapper.Map<Community>(communitydto);
            
            if (id != community.Id || !await _communityService.EntityExists(id) || !_communityService.MeetsTheConstraints(community))
                return BadRequest("Community fields are not correct.");

            if (!_addressService.MeetsTheConstraints(community.Address))
                return BadRequest("Address fields are not correct.");

            community.AddressId = await _addressService.HandleInternalAddressUpdate(community.Address);
            community.Address = await _addressService.GetByIdAsync(community.AddressId);

            await _communityService.UpdateAsync(community);
            await _addressService.DeleteUnusedAddresses();
            return Ok(_mapper.Map<CommunityDTO>(community));
        }

        [HttpPost]
        public async Task<ActionResult<CommunityDTO>> AddCommunity(CommunityDTO communitydto)
        {
            var community = _mapper.Map<Community>(communitydto);

            if (!_communityService.MeetsTheConstraints(community))
                return BadRequest("Community missing mandatory fields.");

            if (!_addressService.MeetsTheConstraints(community.Address))
                return BadRequest("Address fields are not correct.");

            community.AddressId = await _addressService.HandleInternalAddressUpdate(community.Address);
            community.Address = await _addressService.GetByIdAsync(community.AddressId);

            await _communityService.AddAsync(community);
            return CreatedAtAction(nameof(GetCommunity), new { id = community.Id }, _mapper.Map<CommunityDTO>(community));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCommunity(int id)
        {
            if (!await _communityService.EntityExists(id))
            {
                return NotFound();
            }

            await _communityService.DeleteAsync(id);
            await _addressService.DeleteUnusedAddresses();
            return NoContent();
        }
        #endregion


        [HttpGet("filter")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<CommunityDTO>>> GetFilteredCommunities(string? countryCode, string? city, bool open, string? community, int minmember)
        {
            var filter = new CommunityFilter
            {
                CountryCode = countryCode,
                City = city,
                JustOpens = open,
                CommunityNameSubstring = community,
                MinimumMember = minmember
            };

            var communities = await _communityService.GetFilteredCommunitiesAsync(filter);
            if (!communities.Any())
                return NotFound();

            var communitydtos = _mapper.Map<IEnumerable<CommunityDTO>>(communities);
            return Ok(communitydtos);
        }


        #region Member functions
        [HttpGet("{id}/members")]
        public async Task<ActionResult<IEnumerable<MemberDTO>>> GetCommunityMembers(int id)
        {
            if(!await _communityService.EntityExists(id))
                return NotFound();

            var members = await _memberService.GetMembersAsync(id);

            if (!members.Any())
                return NotFound();

            var memberdtos = _mapper.Map<IEnumerable<MemberDTO>>(members);

            return Ok(memberdtos);
        }


        [HttpGet("{id}/members/actives")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<IEnumerable<MemberDTO>>> GetActiveCommunityMembers(int id)
        {
            if (!await _communityService.EntityExists(id))
                return NotFound();

            var members = await _memberService.GetMembersAsync(id, true);

            if (!members.Any())
                return NotFound();

            var memberdtos = _mapper.Map<IEnumerable<MemberDTO>>(members);

            return Ok(memberdtos);
        }


        [HttpPost("{communityId}/add-member")]
        public async Task<IActionResult> AddMember(int communityId, int userId)
        {
            if (!await _communityService.EntityExists(communityId) || !await _userService.EntityExists(userId))
                return BadRequest();

            var member = new Member()
            {
                CommunityId = communityId,
                UserId = userId,
                JoinDate = DateTime.Now,
                Status = MemberStatus.NewComer.ToString()
            };

            await _memberService.AddAsync(member);
            return NoContent();
        }


        [HttpPut("{communityId}/member/{userId}/change-status")]
        public async Task<ActionResult<MemberDTO>> ModifyMemberStatus(int communityId, int userId, string status)
        {
            var member = await _memberService.GetByIdsAsync(communityId, userId);

            if(member == null)
                return NotFound(new Response { Status = "Error", Message = string.Format("User with id {0} is not belongs to community {1}.", userId, communityId) });

            if (!string.IsNullOrEmpty(status))
            {
                if(!Enum.IsDefined(typeof(MemberStatus), status))
                {
                    return BadRequest(new Response { Status = "Error", Message = string.Format("Status field ({0}) is not exist.", status) });
                }
                else
                {
                    member.Status = status;
                    await _memberService.UpdateAsync(member);
                    return Ok(_mapper.Map<MemberDTO>(member));
                }
            }
            return NoContent();
        }

        [HttpPut("{communityId}/member/{userId}/leave")]
        public async Task<IActionResult> QuitMember(int communityId, int userId, DateTime? leaveDate)
        {
            var member = await _memberService.GetByIdsAsync(communityId, userId);

            if (member == null)
                return NotFound(new Response { Status = "Error", Message = string.Format("User with id {0} is not belongs to community {1}.", userId, communityId) });

            if (leaveDate == null)
                member.LeaveDate = DateTime.Now.Date;

            if(leaveDate <= DateTime.Now && leaveDate != DateTime.MinValue)
                member.LeaveDate = leaveDate;

            await _memberService.UpdateAsync(member);
            return Ok(_mapper.Map<MemberDTO>(member));
        }


        [HttpDelete("{communityId}/remove-member")]
        public async Task<IActionResult> DeleteMember(int communityId, int userId)
        {
            var member = await _memberService.GetByIdsAsync(communityId, userId);

            if (member == null)
                return NotFound(new Response { Status = "Error", Message = string.Format("User with id {0} is not belongs to community {1}.", userId, communityId) });

            await _memberService.DeleteAsync(member.Id);
            return NoContent();
        }
        #endregion
    }
}
