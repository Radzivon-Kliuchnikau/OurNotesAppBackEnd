namespace OurNotesAppBackEnd.Models;

public class Comment : BaseModel
{
    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

    public int? ProductId { get; set; }

    public Product? Product { get; set; }
}