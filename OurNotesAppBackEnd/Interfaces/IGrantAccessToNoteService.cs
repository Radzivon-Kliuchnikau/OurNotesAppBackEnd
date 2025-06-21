using OurNotesAppBackEnd.Models;

namespace OurNotesAppBackEnd.Interfaces;

public interface IGrantAccessToNoteService
{
    Task GrantAccessToNoteAsync(string? userId, Note note, IEnumerable<string> emails);
    Task RemoveGrantedAccessFromNoteAsync(string? userId, Note note, IEnumerable<string> emails);
}