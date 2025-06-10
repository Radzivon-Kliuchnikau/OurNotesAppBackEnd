using System.ComponentModel.DataAnnotations;

namespace OurNotesAppBackEnd.Models;

public class Note : BaseModel
{
    [Required]
    [MaxLength(255)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(3000)]
    public string? Content { get; set; } = string.Empty;

    [Required]
    public string AppUserId { get; set; }

    public AppUser AppUser { get; set; }

    public ICollection<NoteAccesses> NoteAccesses { get; set; } = new List<NoteAccesses>();

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}