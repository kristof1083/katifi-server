namespace KatifiWebServer.Models.DTOModels
{
    public class MessDTO
    {
        public int Id { get; set; }
        public string Day { get; set; }

        public TimeOnly StartTime { get; set; }

        public string? Ocasion { get; set; }

        public string? Priest { get; set; }

    }
}
