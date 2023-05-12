using KatifiWebServer.Data.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KatifiWebServer.Models.DatabaseModels;

public class Church : IEntityBase
{
    [Key]
    public int Id { get; set; }

    [Display(Name = "Név"), Required]
    public string Name { get; set; }

    [Display(Name = "Plébános")]
    public string? Vicar { get; set; }


    //Relations
    [Required, ForeignKey("Address")]
    public int AddressId { get; set; }
    public Address Address { get; set; }

    public List<Mess> Messes { get; set; }
}
