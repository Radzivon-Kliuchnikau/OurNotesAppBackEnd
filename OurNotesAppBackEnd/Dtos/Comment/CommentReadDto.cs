using OurNotesAppBackEnd.Models;

namespace OurNotesAppBackEnd.Dtos.Comment;

public class CommentReadDto : BaseModel
{
    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

    public int? ProductId { get; set; }
}