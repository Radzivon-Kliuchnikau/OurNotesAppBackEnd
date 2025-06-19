using Microsoft.EntityFrameworkCore;
using OurNotesAppBackEnd.Data;
using OurNotesAppBackEnd.Interfaces;
using OurNotesAppBackEnd.Models;

namespace OurNotesAppBackEnd.Services;

public class GrantAccessToNoteService(ApplicationDbContext context) : IGrantAccessToNoteService
{
    public async Task GrantAccessToNoteAsync(string? userId, Note note, IEnumerable<string> emails)
    {
        if (userId != note.AppUserId)
        {
            throw new UnauthorizedAccessException("You do not have permission to grant access to this note.");
        }
        
        var users = await context.Users
            .Where(u => emails != null && emails.Contains(u.Email))
            .ToListAsync();

        await context.Entry(note).Reference(n => n.AppUser).LoadAsync();

        foreach (var user in users)
        {
            if (user.Id == note.AppUserId) continue;

            var noteAccess = new NoteAccesses
            {
                AppUserId = user.Id,
                NoteId = note.Id
            };

            context.NoteAccesses.Add(noteAccess);
        }

        await context.SaveChangesAsync();
    }

    public async Task RemoveGrantAccessFromNoteAsync(string? userId, Note note, IEnumerable<string> emails)
    {
        var usersToRemoveAccess = await context.Users
            .Where(u => emails != null && emails.Contains(u.Email))
            .ToListAsync();
        
        
        await context.Entry(note).Reference(n => n.AppUser).LoadAsync();

        foreach (var removedAccessUser in usersToRemoveAccess)
        {
            if (userId != removedAccessUser.Id && userId != note.AppUserId)
            {
                throw new UnauthorizedAccessException("You do not have permission to remove access from this note.");
            }
            
            if (removedAccessUser.Id == note.AppUserId) continue;

            context.NoteAccesses.RemoveRange(context.NoteAccesses.Where(na => na.AppUserId == removedAccessUser.Id && na.NoteId == note.Id));
        }

        await context.SaveChangesAsync();
    }
}