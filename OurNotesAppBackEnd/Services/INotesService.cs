using MongoDB.Bson;
using OurNotesAppBackEnd.Models;

namespace OurNotesAppBackEnd.Services;

public interface INotesService
{
    IEnumerable<Note> GetAllNotes();

    Note? GetNoteById(ObjectId id);

    void AddNote(Note note);

    void EditNote(Note note);

    void DeleteNote(Note note);
}