namespace KatifiWebServer.Models.DTOModels;

public class ChurchDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Vicar { get; set; }
    public AddressDTO Address { get; set; }
}
