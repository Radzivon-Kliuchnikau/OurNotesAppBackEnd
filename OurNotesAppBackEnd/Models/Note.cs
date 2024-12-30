using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.EntityFrameworkCore;

namespace OurNotesAppBackEnd.Models;

[Collection("Notes")]
public class Note
{
    public ObjectId Id { get; set; }

    [Required(ErrorMessage = "You should provide a title for a note")]
    public string Title { get; set; }

    public string Content { get; set; }
}