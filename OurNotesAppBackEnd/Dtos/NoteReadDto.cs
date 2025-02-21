using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;

namespace OurNotesAppBackEnd.Dtos;

public class NoteReadDto
{
    public string Id { get; set; }

    public string Title { get; set; }

    public string Content { get; set; }
    
    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}