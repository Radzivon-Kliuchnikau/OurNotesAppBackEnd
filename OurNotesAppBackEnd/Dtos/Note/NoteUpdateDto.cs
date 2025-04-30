using System.ComponentModel.DataAnnotations;

namespace OurNotesAppBackEnd.Dtos.Note;

public class NoteUpdateDto
{
    [Required(ErrorMessage = "You should provide a title for a note")]
    public string Title { get; set; }

    public string? Content { get; set; }
}