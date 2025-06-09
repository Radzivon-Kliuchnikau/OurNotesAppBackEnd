using System.ComponentModel.DataAnnotations;

namespace OurNotesAppBackEnd.Dtos.Note;

public class NoteCreateDto
{
    [Required(ErrorMessage = "You should provide a title for a note")]
    public string Title { get; set; } = string.Empty;

    public string? Content { get; set; }

    public ICollection<string> UsersWithGrantedAccess { get; set; } = new List<string>();
}