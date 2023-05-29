namespace KatifiWebServer.Models.GoogleModels
{
    public class GoogleEvent
    {
        public string Summary { get; set; }

        public string Description { get; set; }

        public EventDateTime Start { get; set; }
        public EventDateTime End { get; set; }

    }

    public class EventDateTime
    {
        private string timeZone = "Europe/Budapest";

        public string Date { get; set; }
        public string DateTime { get; set; }
        public string TimeZone { get => timeZone; set => timeZone = value; }

    }
}
