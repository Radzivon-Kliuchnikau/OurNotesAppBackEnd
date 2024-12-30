using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OurNotesAppBackEnd.Models;
using OurNotesAppBackEnd.Services;

namespace OurNotesAppBackEnd.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class NotesController : ControllerBase
{
    private readonly INotesService _notesService;

    public NotesController(INotesService notesService)
    {
        _notesService = notesService;
    }
    
    [HttpGet]
    public ActionResult<IEnumerable<Note>> GetAllNotes()
    {
        var notes = _notesService.GetAllNotes();
        
        return Ok(notes);
    }

    [HttpGet("{id}", Name = "GetNoteById")]
    public ActionResult<Note> GetNoteById(string id)
    {
        var note = _notesService.GetNoteById(id);

        if (note == null)
        {
            return NotFound();
        }

        return Ok(note);
    }

    [HttpPost]
    public IActionResult CreateNote([FromBody] Note note)
    {
        _notesService.AddNote(note);
        
        return CreatedAtRoute("GetNoteById", new { id = note.Id.ToString()}, note);
    }

    [HttpPut]
    public IActionResult UpdateNote([FromBody] Note note)
    {
        try
        {
            _notesService.EditNote(note);
            
            return NoContent();
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }

    }

    [HttpDelete]
    public IActionResult DeleteNote([FromBody] Note note)
    {
        try
        {
            _notesService.DeleteNote(note);
            
            return NoContent();
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }
}