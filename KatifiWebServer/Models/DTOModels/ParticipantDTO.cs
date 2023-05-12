namespace KatifiWebServer.Models.DTOModels
{
    public class ParticipantDTO
    {
        public int Id { get; set; }
        public DateTime ApplicationDate { get; set; }
        public string? UserFullName { get; set; }
        public int UserAge { get; set; }

        public int UserId { get; set; }
        public int EventId { get; set; }
    }
}
