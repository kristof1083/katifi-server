using KatifiWebServer.Models.DTOModels;
using System.ComponentModel.DataAnnotations;

namespace KatifiWebServer.Models.SecurityModels
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "User Name is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Lastname is required")]
        public string Lastname { get; set; }

        [Required(ErrorMessage = "FirstName is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Binary gender selection is required")]
        public char Gender { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public DateOnly BornDate { get; set; }

        [Required(ErrorMessage = "You have to agree to the end user's terms")]
        public bool AgreeTerm { get; set; }

        public string? UserToken { get; set; }

        public int? AddressID { get; set; }
    }
}
