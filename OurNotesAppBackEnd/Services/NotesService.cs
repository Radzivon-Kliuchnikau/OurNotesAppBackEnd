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

    public async Task<IEnumerable<Note>> GetAllNotes()
    {
        return await _repository.GetAllEntitiesAsync();
    }

    public async Task<Note?> GetNoteById(string id)
    {
        return await _repository.GetEntityByIdAsync(id);
    }

    public async Task AddNote(Note note)
    {
        await _repository.AddEntityAsync(note);
    }

    public async Task EditNote(Note note)
    {
        await _repository.EditEntity(note);
    }

    public async Task DeleteNote(Note note)
    {
        await _repository.DeleteEntity(note);
    }
}