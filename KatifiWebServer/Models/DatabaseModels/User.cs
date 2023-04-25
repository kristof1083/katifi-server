using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using KatifiWebServer.Data.Base;
using System.Diagnostics.CodeAnalysis;

namespace KatifiWebServer.Models.DatabaseModels;

public class User : IEntityBase
{
    [Key]
    public int Id { get; set; }

    [Display(Name = "Felhasználónév"), Required]
    public string Username { get; set; }

    [Display(Name = "Jelszó"), Required]
    public string Password { get; set; }

    [Display(Name = "Regisztráció dátuma"), NotNull]
    public DateTime RegistrationDate { get; set; }


    [Display(Name = "Vezetéknév"), Required]
    public string Lastname { get; set; }

    [Display(Name = "Keresztnév"), Required]
    public string FirstName { get; set; }

    [NotMapped]
    public string FullName { get => $"{Lastname} {FirstName}"; }

    [Display(Name = "Nem"), Required]
    public char Gender { get; set; }

    [Display(Name = "Születési dátum"), Required, Column("BornDate")]
    public DateTime BornDatetime { get; set; }

    [NotMapped]
    public DateOnly BornDate { get => DateOnly.FromDateTime(BornDatetime); set => BornDatetime = value.ToDateTime(new TimeOnly(0,0)); }

    [EmailAddress]
    public string? Email { get; set; }

    [Display(Name = "Felhasználó feltételek elfogadása"), Required]
    public bool AgreeTerm { get; set; }

    [NotMapped]
    public string? UserToken { get; set; }


    //Relations

    [Required, ForeignKey("Role")]
    public int RoleId { get; set; }
    public Role Role { get; set; }


    [ForeignKey("Address")]
    public int? AddressID { get; set; }
    public Address? Address { get; set; }


    public List<Member> Members { get; set; }

    public List<Participant> Participants { get; set; }
}
