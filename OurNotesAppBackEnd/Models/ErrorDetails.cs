
using System.Text.Json;

namespace OurNotesAppBackEnd.Models;

public class ErrorDetails
{
    public int StatusCode { get; set; }

    public string? ErrorMessage { get; set; }

    public override string ToString() => JsonSerializer.Serialize(this);
}