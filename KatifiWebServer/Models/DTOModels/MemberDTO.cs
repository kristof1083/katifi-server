using KatifiWebServer.Data.Enums;

namespace KatifiWebServer.Models.DTOModels;

public class MemberDTO
{
    public int Id { get; set; }

    public string CommunityName { get; set; }
    public string UserName { get; set; }
    public string Status { get; set; }
    public DateTime JoinDate { get; set; }
    public DateTime? LeaveDate { get; set; }


}
