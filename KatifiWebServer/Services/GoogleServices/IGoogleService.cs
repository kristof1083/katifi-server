using KatifiWebServer.Models.DTOModels;

namespace KatifiWebServer.Services.GoogleServices
{
    public interface IGoogleService
    {
        //public Task<IActionResult> RefreshToken();

        public Task<bool> CreateEvent(EventDTO newevent);
        public Task GetGoogleTokens(string code);
        public string? GetSignUri();

        public Dictionary<string, string>? GetProjectData();
        public Dictionary<string, string>? GetCredentials();
        public Dictionary<string, object>? GetTokens();
    }
}
