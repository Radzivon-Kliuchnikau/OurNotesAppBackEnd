using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using OurNotesAppBackEnd.Models;

namespace OurNotesAppBackEnd.Data.Repository;

public class NoteMongoDbRepository : INoteMongoDbRepository
{
    private readonly NotesAppDbContext _context;

    public NoteMongoDbRepository(NotesAppDbContext context)
    {
        _context = context;
    }
    
    public IEnumerable<Note> GetAllEntities()
    {
        return _context.Notes.OrderByDescending(n => n.Id).AsNoTracking().AsEnumerable();
    }

    public Note? GetEntityById(ObjectId id)
    {
        return _context.Notes.FirstOrDefault(n => n.Id == id);
    }

    public void AddEntity(Note entity)
    {
        _context.Notes.Add(entity);
        
        _context.ChangeTracker.DetectChanges();
        Console.WriteLine(_context.ChangeTracker.DebugView.LongView); // TODO: We won't use it in production
        
        _context.SaveChanges();
    }

    public void EditEntity(Note entity)
    {
        var noteToBeUpdated = _context.Notes.FirstOrDefault(n => n.Id == entity.Id);

        if (noteToBeUpdated != null)
        {
            noteToBeUpdated.Title = entity.Title;
            noteToBeUpdated.Content = entity.Content;

            _context.Notes.Update(noteToBeUpdated);
            _context.ChangeTracker.DetectChanges();
            Console.WriteLine(_context.ChangeTracker.DebugView.LongView);
            
            _context.SaveChanges();
        }
        else
        {
            throw new ArgumentException("There is no such note to be updated");
        }
    }

    public void DeleteEntity(Note entity)
    {
        var noteToDelete = _context.Notes.FirstOrDefault(n => n.Id == entity.Id);

        if (noteToDelete != null)
        {
            _context.Notes.Remove(entity);
            _context.ChangeTracker.DetectChanges();
            Console.WriteLine(_context.ChangeTracker.DebugView.LongView);
            
            _context.SaveChanges();
        }
        else
        {
            throw new ArgumentException("There is no such note to be removed");
        }
        
    }
}