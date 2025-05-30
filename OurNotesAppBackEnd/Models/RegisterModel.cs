using System.ComponentModel.DataAnnotations;

namespace OurNotesAppBackEnd.Models;

public class RegisterModel
{
    public string UserName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}