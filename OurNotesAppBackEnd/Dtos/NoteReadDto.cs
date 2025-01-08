using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;

namespace OurNotesAppBackEnd.Dtos;

public class NoteReadDto
{
    public ObjectId Id { get; set; }

    public string Title { get; set; }

    public string Content { get; set; }
}