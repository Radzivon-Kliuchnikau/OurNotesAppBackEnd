using OurNotesAppBackEnd.Models;

namespace OurNotesAppBackEnd.Dtos.Product;

public class ProductReadDto
{
    public int  Id { get; set; }
    
    public string Name { get; set; } = string.Empty;

    public string Brand { get; set; } = string.Empty;

    public string Url { get; set; } = string.Empty;

    public string Image { get; set; } = string.Empty;

    public decimal RawPrice { get; set; }

    public List<Comment> Comments { get; set; } = [];
}