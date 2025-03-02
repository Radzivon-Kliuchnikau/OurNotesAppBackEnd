using System.ComponentModel.DataAnnotations;

namespace OurNotesAppBackEnd.Dtos;

public class NoteCreateDto
{
    [Required(ErrorMessage = "You should provide a title for a note")]
    public string Title { get; set; }

    public string? Content { get; set; }
}