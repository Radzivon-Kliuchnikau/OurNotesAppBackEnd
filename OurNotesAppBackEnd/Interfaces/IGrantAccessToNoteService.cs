using OurNotesAppBackEnd.Models;

namespace OurNotesAppBackEnd.Interfaces;

public interface IGrantAccessToNoteService
{
    Task GrantAccessToNoteAsync(Note note, IEnumerable<string> emails);
}