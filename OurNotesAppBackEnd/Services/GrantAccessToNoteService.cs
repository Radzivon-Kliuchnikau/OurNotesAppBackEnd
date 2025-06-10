using Microsoft.EntityFrameworkCore;
using OurNotesAppBackEnd.Data;
using OurNotesAppBackEnd.Interfaces;
using OurNotesAppBackEnd.Models;

namespace OurNotesAppBackEnd.Services;

public class GrantAccessToNoteService(ApplicationDbContext context) : IGrantAccessToNoteService
{
    public async Task GrantAccessToNoteAsync(Note note, ICollection<string>? emails)
    {
        var users = await context.Users
            .Where(u => emails != null && emails.Contains(u.Email))
            .ToListAsync();
        
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
}