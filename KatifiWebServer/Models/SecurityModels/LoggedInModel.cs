using KatifiWebServer.Models.DTOModels;

namespace KatifiWebServer.Models.SecurityModels
{
    public class LoggedInModel
    {
        public string? JwtToken { get; set; }
        public UserDTO? LoggedInUser { get; set; }
    }
}
