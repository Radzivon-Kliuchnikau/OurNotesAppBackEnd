using System.ComponentModel.DataAnnotations;

namespace OurNotesAppBackEnd.Dtos.Account;

public class LoginRequestDto
{
    [Required] 
    public string Email { get; set; } = string.Empty;

    [Required] 
    public string Password { get; set; } = string.Empty;
}