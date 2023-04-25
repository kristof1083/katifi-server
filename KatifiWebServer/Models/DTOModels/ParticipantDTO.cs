namespace KatifiWebServer.Models.DTOModels
{
    public class ParticipantDTO
    {
        public int Id { get; set; }
        public DateTime ApplicationDate { get; set; }
        public UserDTO User { get; set; }

        public EventDTO Event { get; set; }
    }
}
