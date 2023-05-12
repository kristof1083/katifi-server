namespace KatifiWebServer.Models.DTOModels
{
    public class UserDTO
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Lastname { get; set; }

        public string FirstName { get; set; }

        public char Gender { get; set; }

        public DateOnly BornDate { get; set; }

        public string? Email { get; set; }

        public bool AgreeTerm { get; set; }

        public string? UserToken { get; set; }

        public AddressDTO Address { get; set; }
    }
}
