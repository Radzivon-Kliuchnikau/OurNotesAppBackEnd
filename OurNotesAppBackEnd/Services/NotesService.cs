using MongoDB.Bson;
using OurNotesAppBackEnd.Data.Repository;
using OurNotesAppBackEnd.Models;

namespace OurNotesAppBackEnd.Services;

public class NotesService : INotesService
{
    private readonly INoteMongoDbRepository _repository;

    // ReSharper disable once ConvertToPrimaryConstructor
    public NotesService(INoteMongoDbRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<Note> GetAllNotes()
    {
        return _repository.GetAllEntities();
    }

    public Note? GetNoteById(string id) // TODO: This about removing creation on ObjectId from here
    {
        var objectId = new ObjectId(id);
        return _repository.GetEntityById(objectId);
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