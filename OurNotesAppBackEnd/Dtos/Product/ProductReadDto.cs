using OurNotesAppBackEnd.Dtos.Comment;
using OurNotesAppBackEnd.Models;

namespace OurNotesAppBackEnd.Dtos.Product;

public class ProductReadDto : BaseModel
{
    public string Name { get; set; } = string.Empty;

    public string Brand { get; set; } = string.Empty;

    public string Url { get; set; } = string.Empty;

    public string ImageUrl { get; set; } = string.Empty;

    public decimal RawPrice { get; set; }
    
    public List<CommentReadDto> Comments { get; set; } = [];
}