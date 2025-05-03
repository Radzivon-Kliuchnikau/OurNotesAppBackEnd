using System.ComponentModel.DataAnnotations;

namespace OurNotesAppBackEnd.Dtos.Account;

public class RegistrationModelDto
{
    [Required]
    [MinLength(3, ErrorMessage = "User name should have minimum 3 characters")]
    public string UserName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;

    [Compare("Password", ErrorMessage = "The confirmation password do not match with password")]
    public string ConfirmPassword { get; set; } = string.Empty;
}