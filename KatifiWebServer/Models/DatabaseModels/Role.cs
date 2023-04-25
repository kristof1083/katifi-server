using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using KatifiWebServer.Data.Enums;
using KatifiWebServer.Data.Base;

namespace KatifiWebServer.Models.DatabaseModels;

public class Role : IEntityBase
{
    [Key]
    public int Id { get; set; }

    [Required, Column(TypeName = "nvarchar(20)"), Display(Name = "Jogosultság elnevezése")]
    public IdentityRoles Name { get; set; }

    //Relations
    public List<User> Users { get; set; }

}
