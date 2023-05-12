using KatifiWebServer.Data.Enums;
using KatifiWebServer.Data.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KatifiWebServer.Models.DatabaseModels;
public class Member : IEntityBase
{
    [Key]
    public int Id { get; set; }

    [Required, Display(Name = "Tagsági státusz")]
    public string Status { get; set; }

    [Required, Display(Name = "Csatlakozás dátuma")]
    public DateTime JoinDate { get; set; }

    [Display(Name = "Kilépés dátuma")]
    public DateTime? LeaveDate { get; set; }


    //Relations

    [Required, ForeignKey("Community")]
    public int CommunityId { get; set; }
    public Community Community { get; set; }


    [Required, ForeignKey("User")]
    public int UserId { get; set; }
    public AppUser User { get; set; }
}
