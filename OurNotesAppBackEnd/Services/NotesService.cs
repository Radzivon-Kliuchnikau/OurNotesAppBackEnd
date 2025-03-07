using MongoDB.Bson;
using OurNotesAppBackEnd.Data.Repository;
using OurNotesAppBackEnd.Models;

namespace OurNotesAppBackEnd.Services;

public class NotesService : INotesService
{
    private readonly INoteSqlServerRepository _repository;

    // ReSharper disable once ConvertToPrimaryConstructor
    public NotesService(INoteSqlServerRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<Note> GetAllNotes()
    {
        return _repository.GetAllEntities();
    }

    public Note? GetNoteById(string id)
    {
        return _repository.GetEntityById(id);
    }

    public void AddNote(Note note)
    {
        _repository.AddEntity(note);
    }

    public void EditNote(Note note)
    {
        _repository.EditEntity(note);
    }

    public void DeleteNote(Note note)
    {
        _repository.DeleteEntity(note);
    }
}