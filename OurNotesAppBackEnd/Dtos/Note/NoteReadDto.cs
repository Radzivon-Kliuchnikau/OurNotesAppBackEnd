using OurNotesAppBackEnd.Models;

namespace OurNotesAppBackEnd.Dtos.Note;

public class NoteReadDto : BaseModel
{
    public string Title { get; set; }

    public string Content { get; set; }
    
    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}