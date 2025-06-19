using Microsoft.AspNetCore.Identity;

namespace OurNotesAppBackEnd.Models;

public class AppUser : IdentityUser
{
    public IList<Note> Notes { get; set; } = new List<Note>();
    public IList<NoteAccesses> NoteAccesses { get; set; } = new List<NoteAccesses>();
}