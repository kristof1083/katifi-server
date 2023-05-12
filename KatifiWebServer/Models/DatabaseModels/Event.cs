using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using KatifiWebServer.Data.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KatifiWebServer.Models.DatabaseModels;

public class Event : IEntityBase
{

    [Key]
    public int Id { get; set; }

    [Display(Name = "Név"), Required]
    public string Name { get; set; }

    [Display(Name = "Dátum"), Required]
    public DateTime Date { get; set; }

    [Display(Name = "Regisztrációs határidő"), Required]
    public DateTime RegistrationDeadline { get; set; }

    [Display(Name = "Ár"), Required]
    public int Fee { get; set; }

    [Display(Name = "Szervező"), Required]
    public string Organizer { get; set; }

    [Display(Name = "Max résztvevők")]
    public int? MaxParticipant { get; set; }


    [Display(Name = "Kép mappa url"), NotMapped]
    public string PictureFolderUrl => Name + "_" + Date.Date.ToString();

    [NotMapped]
    public int ParticipantCount { get => Participants == null ? 0 : Participants.Count; }


    //Relations

    [Required, ForeignKey("Address")]
    public int AddressId { get; set; }
    public Address Address { get; set; }
    public List<Participant> Participants { get; set; }
}
