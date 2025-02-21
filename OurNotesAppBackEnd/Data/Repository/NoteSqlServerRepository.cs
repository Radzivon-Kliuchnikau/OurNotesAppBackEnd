using Microsoft.EntityFrameworkCore;
using OurNotesAppBackEnd.Models;

namespace OurNotesAppBackEnd.Data.Repository;

public class NoteSqlServerRepository : INoteSqlServerRepository
{
    private readonly NotesAppDbContext _context;

    public NoteSqlServerRepository(NotesAppDbContext context)
    {
        _context = context;
    }
    public IEnumerable<Note> GetAllEntities()
    {
        return _context.Notes.OrderByDescending(n => n.Id).AsNoTracking().AsEnumerable();
    }

    public Note? GetEntityById(string id)
    {
        return _context.Notes.FirstOrDefault(n => n.Id == id);
    }

    public void AddEntity(Note entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        _context.Notes.Add(entity);

        _context.ChangeTracker.DetectChanges();
        Console.WriteLine(_context.ChangeTracker.DebugView.LongView); // TODO: We won't use it in production

        _context.SaveChanges();
    }

    public void EditEntity(Note entity)
    {
        _context.Notes.Update(entity);
        _context.ChangeTracker.DetectChanges();
        Console.WriteLine(_context.ChangeTracker.DebugView.LongView);

        _context.SaveChanges();
    }

    public void DeleteEntity(Note entity)
    {
        if (entity == null)
        {
            throw new ArgumentException(nameof(entity));
        }
        
        _context.Notes.Remove(entity);
        _context.ChangeTracker.DetectChanges();
        Console.WriteLine(_context.ChangeTracker.DebugView.LongView);

        _context.SaveChanges();
    }
}