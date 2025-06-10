using OurNotesAppBackEnd.Dtos.User;
using OurNotesAppBackEnd.Models;

namespace OurNotesAppBackEnd.Dtos.Note;

public class NoteReadDto : BaseModel
{
    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;
    
    public AuthorReadDto AuthorReadDto { get; set; }
    
    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}