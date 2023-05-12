namespace KatifiWebServer.Models.FilterModels
{
    public class CommunityFilter
    {
        public string? CountryCode { get; set; } = string.Empty;
        public string? City { get; set; } = string.Empty;
        public bool JustOpens { get; set; } = false;
        public string? CommunityNameSubstring { get; set; } = string.Empty;
        public int MinimumMember { get; set; } = 0;
    }
}
