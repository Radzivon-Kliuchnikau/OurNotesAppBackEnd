using Microsoft.EntityFrameworkCore;
using OurNotesAppBackEnd.Interfaces;
using OurNotesAppBackEnd.Models;

namespace OurNotesAppBackEnd.Data.Repositories;

public class NoteRepository(ApplicationDbContext context) : BaseRepository<Note, Guid>(context), INoteRepository
{
    public new async Task<Note?> GetEntityByIdAsync(Guid id)
    {
        return await context.Notes
            .Include(n => n.NoteAccesses)
            .Include(n => n.AppUser)
            .FirstOrDefaultAsync(n => n.Id == id);
    }
    
    public async Task<IEnumerable<Note>> GetNotesUserHaveAccessTo(string authorId)
    {
        return await context.Notes
            .Include(n => n.NoteAccesses)
            .Include(n => n.AppUser)
            .Where(n => n.AppUserId == authorId || n.NoteAccesses.Any(na => na.AppUserId == authorId))
            .ToListAsync();
    }
}