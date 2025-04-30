using System.ComponentModel.DataAnnotations;

namespace OurNotesAppBackEnd.Models;

public class Note : BaseModel
{
    [Required]
    [MaxLength(255)]
    public string Title { get; set; }

    public string? Content { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}