using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using OurNotesAppBackEnd.Models;

namespace OurNotesAppBackEnd.Dtos.Product;

public class ProductCreateDto
{
    [Required]
    [MaxLength(50, ErrorMessage = "Name of the product shouldn't be more then 20 characters")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(50, ErrorMessage = "Brand of the product shouldn't be more then 20 characters")]
    public string Brand { get; set; } = string.Empty;

    [Required]
    public string Url { get; set; } = string.Empty;
    
    public string ImageUrl { get; set; } = string.Empty;

    [Required]
    [Range(1, 100000000)]
    public decimal RawPrice { get; set; }
}