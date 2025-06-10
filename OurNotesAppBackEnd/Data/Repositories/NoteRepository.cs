using Microsoft.EntityFrameworkCore;
using OurNotesAppBackEnd.Interfaces;
using OurNotesAppBackEnd.Models;

namespace OurNotesAppBackEnd.Data.Repositories;

public class NoteRepository(ApplicationDbContext context) : BaseRepository<Note, Guid>(context), INoteRepository
{
    public async Task<IEnumerable<Note>> GetNotesUserHaveAccessTo(string authorId)
    {
        return await context.Notes
            .Include(n => n.NoteAccesses)
            .Where(n => n.AppUserId == authorId || n.NoteAccesses.Any(na => na.AppUserId == authorId))
            .Where(note => note.AppUserId == authorId)
            .ToListAsync();
    }
}