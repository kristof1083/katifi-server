namespace KatifiWebServer.Models.DTOModels;

public class AddressDTO
{
    public int Id { get; set; }
    public string CountryCode { get; set; }
    public string County { get; set; }
    public int? PostCode { get; set; }
    public string City { get; set; }
    public string? Street { get; set; }
    public int? HouseNumber { get; set; }
}
