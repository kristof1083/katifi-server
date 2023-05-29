namespace KatifiWebServer.Models.GoogleModels
{
    public class GoogleCalendar
    {
        private string timeZone = "Europe/Budapest";

        public string Id { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string TimeZone { get => timeZone; set => timeZone = value; }

    }
}
