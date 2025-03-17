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
    public async Task<IEnumerable<Note>> GetAllEntitiesAsync()
    {
        return await _context.Notes.OrderByDescending(n => n.Id).AsNoTracking().ToListAsync();
    }

    public async Task<Note?> GetEntityByIdAsync(string id)
    {
        return await _context.Notes.FirstOrDefaultAsync(n => n.Id == id);
    }

    public async Task AddEntityAsync(Note entity)
    {
        await _context.Notes.AddAsync(entity);

        _context.ChangeTracker.DetectChanges();
        Console.WriteLine(_context.ChangeTracker.DebugView.LongView); // TODO: We won't use it in production

        await _context.SaveChangesAsync();
    }

    public async Task EditEntity(Note entity)
    {
        _context.Notes.Update(entity);
        _context.ChangeTracker.DetectChanges();
        Console.WriteLine(_context.ChangeTracker.DebugView.LongView);

        await _context.SaveChangesAsync();
    }

    public async Task DeleteEntity(Note entity)
    {
        _context.Notes.Remove(entity);
        _context.ChangeTracker.DetectChanges();
        Console.WriteLine(_context.ChangeTracker.DebugView.LongView);

        await _context.SaveChangesAsync();
    }
}