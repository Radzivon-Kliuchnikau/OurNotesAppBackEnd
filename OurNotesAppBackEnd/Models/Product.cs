using System.ComponentModel.DataAnnotations.Schema;

namespace OurNotesAppBackEnd.Models;

public class Product : BaseModel
{
    public string Name { get; set; } = string.Empty;

    public string Brand { get; set; } = string.Empty;

    public string Url { get; set; } = string.Empty;

    public string ImageUrl { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal RawPrice { get; set; }

    public List<Comment> Comments { get; set; } = [];
}