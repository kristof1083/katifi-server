using KatifiWebServer.Models.DTOModels;
using KatifiWebServer.Services.GoogleServices;
using Microsoft.AspNetCore.Mvc;

namespace KatifiWebServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoogleController : Controller
    {
        readonly IGoogleService _service;

        public GoogleController(IGoogleService googleService)
        {
            _service = googleService;
        }

        [HttpGet("callback")]
        public async Task CallBack(string code, string state, string error = "")
        {
            if (string.IsNullOrEmpty(error))
            {
                await _service.GetGoogleTokens(code);
            }
        }

        [HttpGet("oauth")]
        public ActionResult<string> GetOAuthLink()
        {
            var uri = _service.GetSignUri();
            if (uri is null)
                return NotFound();

            return Ok(uri);
        }

        [HttpPost("create-event")]
        public async Task<IActionResult> CreateEvent(EventDTO newEvent)
        {
            bool success = await _service.CreateEvent(newEvent);
            if (success)
                return Ok();

            return BadRequest("nope");
        }
    }
}
