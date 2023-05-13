using KatifiWebServer.Data.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KatifiWebServer.Models.DatabaseModels;

public class Community : IEntityBase
{
    [Key]
    public int Id { get; set; }

    [Display(Name = "Név"), Required]
    public string Name { get; set; }

    [Display(Name = "Nyitott"), Required]
    public bool IsOpen { get; set; }

    [Display(Name = "Kép elérési átvonal")]
    public string? ImageUrl { get; set; }

    [NotMapped]
    public int MemberCount { get => Members == null ? 0 : Members.Count; }


    //Relations
    [Required, ForeignKey("Address")]
    public int AddressId { get; set; }
    public Address Address { get; set; }


    public List<Member> Members { get; set; }
}
