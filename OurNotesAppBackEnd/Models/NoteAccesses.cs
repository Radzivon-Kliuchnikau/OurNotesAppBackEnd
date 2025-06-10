namespace OurNotesAppBackEnd.Models;

public class NoteAccesses
{
    public Guid NoteId { get; set; }
    public Note Note { get; set; }

    public string AppUserId { get; set; }
    public AppUser AppUser { get; set; }
}