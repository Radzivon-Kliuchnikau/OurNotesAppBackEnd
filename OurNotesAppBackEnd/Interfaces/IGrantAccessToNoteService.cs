using OurNotesAppBackEnd.Models;

namespace OurNotesAppBackEnd.Interfaces;

public interface IGrantAccessToNoteService
{
    Task GrantAccessToNoteAsync(string? userId, Note note, IEnumerable<string> emails);
    Task RemoveGrantAccessFromNoteAsync(string? userId, Note note, IEnumerable<string> emails);
}