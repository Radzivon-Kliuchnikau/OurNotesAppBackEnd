using Microsoft.EntityFrameworkCore;
using OurNotesAppBackEnd.Interfaces;

namespace OurNotesAppBackEnd.Data.Repositories;

public class BaseRepository<T, K> : IBaseRepository<T, K> where T : class where K : notnull
{
    private readonly ApplicationDbContext _context;

    public BaseRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<T>> GetAllEntitiesAsync()
    {
        return await _context.Set<T>().OrderByDescending<T, K>(n => EF.Property<K>(n, "Id")).AsNoTracking().ToListAsync();
    }

    public async Task<T?> GetEntityByIdAsync(K id)
    {
        return await _context.Set<T>().FirstOrDefaultAsync(n => EF.Property<K>(n, "Id").Equals(id));
    }

    public async Task AddEntityAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);

        _context.ChangeTracker.DetectChanges();
        Console.WriteLine(_context.ChangeTracker.DebugView.LongView); // TODO: We won't use it in production

        await _context.SaveChangesAsync();
    }

    public async Task EditEntity(T entity)
    {
        _context.Set<T>().Update(entity);
        _context.ChangeTracker.DetectChanges();
        Console.WriteLine(_context.ChangeTracker.DebugView.LongView);

        await _context.SaveChangesAsync();
    }

    public async Task DeleteEntity(T entity)
    {
        _context.Set<T>().Remove(entity);
        _context.ChangeTracker.DetectChanges();
        Console.WriteLine(_context.ChangeTracker.DebugView.LongView);

        await _context.SaveChangesAsync();
    }

    public async Task<bool> DoesEntityExists(K id)
    {
        return await _context.Set<T>().AnyAsync(e => EF.Property<K>(e, "Id").Equals(id));
    }
}