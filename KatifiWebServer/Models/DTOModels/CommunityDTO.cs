namespace KatifiWebServer.Models.DTOModels;

public class CommunityDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsOpen { get; set; }

    public AddressDTO Address { get; set; }

    public int MemberCount { get; set; }

}
