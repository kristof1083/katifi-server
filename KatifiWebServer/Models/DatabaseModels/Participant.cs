using KatifiWebServer.Data.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace KatifiWebServer.Models.DatabaseModels;

public class Participant : IEntityBase
{
    [Key]
    public int Id { get; set; }

    [Display(Name = "Jelentkezés dátuma"), Required]
    public DateTime ApplicationDate { get; set; }


    //Relations
    [Required, ForeignKey("Event")]
    public int EventId { get; set; }
    public Event Event { get; set; }


    [Required, ForeignKey("User")]
    public int UserId { get; set; }
    public AppUser User { get; set; }
}
