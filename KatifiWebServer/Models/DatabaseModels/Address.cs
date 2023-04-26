using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using KatifiWebServer.Data.Base;

namespace KatifiWebServer.Models.DatabaseModels;

public class Address : IEntityBase
{
    [Key]
    public int Id { get; set; }

    [Display(Name = "OrszágKód"), MaxLength(3), Required]
    public string CountryCode { get; set; }

    [Display(Name = "Megye"), Required]
    public string County { get; set; }

    [Display(Name = "Irányítószám"), Range(1000, 9999)]
    public int? PostCode { get; set; }

    [Display(Name = "Település"), Required]
    public string City { get; set; }

    [Display(Name = "Út/Utca")]
    public string? Street { get; set; }

    [Display(Name = "Házszám")]
    public int? HouseNumber { get; set; }


    //Relations
    public List<AppUser> Users { get; set; }
    public List<Community> Communities { get; set; }
    public Church Church { get; set; }
}
