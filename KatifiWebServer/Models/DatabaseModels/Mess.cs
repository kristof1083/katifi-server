using KatifiWebServer.Data.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KatifiWebServer.Models.DatabaseModels;

public class Mess : IEntityBase
{
    [Key]
    public int Id { get; set; }

    [Display(Name = "Nap"), Required]
    public string Day { get; set; }

    [Display(Name = "Kezdés"), Required, Column("StartTime")]
    public DateTime StartDatetime { get; set; }

    [NotMapped]
    public TimeOnly StartTime { get => TimeOnly.FromDateTime(StartDatetime); set => StartDatetime = new DateTime(2000,01,01) + value.ToTimeSpan(); }

    [Display(Name = "Alkalom")]
    public string? Ocasion { get; set; }

    [Display(Name = "Miséző pap")]
    public string? Priest { get; set; }


    //Relations
    [Required, ForeignKey("Church")]
    public int ChurchId { get; set; }
    public Church Church { get; set; }

}
