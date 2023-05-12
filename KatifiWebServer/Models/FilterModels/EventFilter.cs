namespace KatifiWebServer.Models.FilterModels
{
    public class EventFilter
    {
        public string? CountryCode { get; set; } = string.Empty;
        public string? City { get; set; } = string.Empty;
        public bool JustActives { get; set; } = false;
        public string? EventNameSubstring { get; set; } = string.Empty;
        public int MinimumParticipant { get; set; } = 0;

        public DateTime? EarliestEventDate { get; set; } = DateTime.MinValue;
        public DateTime? LatestEventDate { get; set; } = DateTime.MinValue;

    }
}
