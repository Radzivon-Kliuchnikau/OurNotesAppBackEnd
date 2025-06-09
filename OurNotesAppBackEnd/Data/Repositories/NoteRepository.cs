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
            .Where(n => n.AppUserId.ToString() == authorId || n.NoteAccesses.Any(na => na.AppUserId.ToString() == authorId))
            .Where(note => note.AppUserId.ToString() == authorId)
            .ToListAsync();
    }
}