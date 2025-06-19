using OurNotesAppBackEnd.Models;

namespace OurNotesAppBackEnd.Dtos.User;

public class AuthorReadDto : BaseModel
{
    public string UserName { get; set; }

    public string Email { get; set; }
}