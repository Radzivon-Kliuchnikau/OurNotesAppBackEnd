using OurNotesAppBackEnd.Models;

namespace OurNotesAppBackEnd.Interfaces;

public interface INotesService
{
    Task<IEnumerable<Note>> GetAllNotes();

    Task<Note?> GetNoteById(string id);

    Task AddNote(Note note);

    Task EditNote(Note note);

    Task DeleteNote(Note note);
}