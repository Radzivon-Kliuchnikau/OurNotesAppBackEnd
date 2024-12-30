using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OurNotesAppBackEnd.Models;
using OurNotesAppBackEnd.Services;

namespace OurNotesAppBackEnd.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class NotesController : ControllerBase
{
    private readonly INotesService _notesService;

    public NotesController(INotesService notesService)
    {
        _notesService = notesService;
    }
    
    [HttpGet]
    public IActionResult GetNotes()
    {
        var notes = _notesService.GetAllNotes();
        
        return Ok(notes);
    }

    [HttpPost("createnote")]
    public IActionResult CreateNote([FromBody] Note note)
    {
        _notesService.AddNote(note);
        return Ok("Note created");
    }
}