using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using KatifiWebServer.Data.Base;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Identity;

namespace KatifiWebServer.Models.DatabaseModels;

public class AppUser : IdentityUser<int>, IEntityBase
{
    [Display(Name = "Vezetéknév"), Required, PersonalData]
    public string Lastname { get; set; }

    [Display(Name = "Keresztnév"), Required, PersonalData]
    public string FirstName { get; set; }

    [Display(Name = "Regisztráció dátuma")]
    public DateTime RegistrationDate { get; set; }

    [NotMapped]
    public string FullName { get => $"{Lastname} {FirstName}"; }

    [Display(Name = "Nem"), Required]
    public char Gender { get; set; }

    [Display(Name = "Születési dátum"), Required, Column("BornDate"), ProtectedPersonalData]
    public DateTime BornDatetime { get; set; }

    [NotMapped]
    public DateOnly BornDate { get => DateOnly.FromDateTime(BornDatetime); set => BornDatetime = value.ToDateTime(new TimeOnly(0,0)); }

    [Display(Name = "Felhasználó feltételek elfogadása"), Required]
    public bool AgreeTerm { get; set; }

    [NotMapped]
    public string? UserToken { get; set; }


    //Relations

    [ForeignKey("Address"), ProtectedPersonalData]
    public int? AddressID { get; set; }
    public Address? Address { get; set; }


    public List<Member> Members { get; set; }

    public List<Participant> Participants { get; set; }
}
