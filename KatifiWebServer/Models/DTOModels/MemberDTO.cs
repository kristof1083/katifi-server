namespace KatifiWebServer.Models.DTOModels;

public class MemberDTO
{
    public int Id { get; set; }

    public int CommunityId { get; set; }
    public int UserId { get; set; }
    public string? UserFullName { get; set; }
    public int UserAge { get; set; }
    public string Status { get; set; }
    public DateTime JoinDate { get; set; }
    public DateTime? LeaveDate { get; set; }


}
