namespace KatifiWebServer.Models.DTOModels;

public class EventDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime Date { get; set; }
    public DateTime RegistrationDeadline { get; set; }
    public int Fee { get; set; }
    public string Organizer { get; set; }
    public int? MaxParticipant { get; set; }

    public AddressDTO Address { get; set; }
    public int ParticipantCount { get; set; }
}
