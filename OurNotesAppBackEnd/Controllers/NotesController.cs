using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OurNotesAppBackEnd.Dtos;
using OurNotesAppBackEnd.Models;
using OurNotesAppBackEnd.Services;

namespace OurNotesAppBackEnd.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class NotesController : ControllerBase
{
    private readonly INotesService _notesService;
    private readonly IMapper _mapper;

    public NotesController(INotesService notesService, IMapper mapper)
    {
        _notesService = notesService;
        _mapper = mapper;
    }
    
    [HttpGet]
    public ActionResult<IEnumerable<NoteReadDto>> GetAllNotes()
    {
        var notes = _notesService.GetAllNotes();
        
        return Ok(_mapper.Map<IEnumerable<NoteReadDto>>(notes));
    }

    [HttpGet("{id}", Name = "GetNoteById")]
    public ActionResult<NoteReadDto> GetNoteById(string id)
    {
        var note = _notesService.GetNoteById(id);

        if (note == null)
            return NotFound();
        {
        }

        return Ok(_mapper.Map<NoteReadDto>(note));
    }

    [HttpPost]
    public ActionResult<NoteReadDto> CreateNote([FromBody] NoteCreateDto note)
    {
        var noteModel = _mapper.Map<Note>(note);
        _notesService.AddNote(noteModel);

        var noteReadDto = _mapper.Map<NoteReadDto>(noteModel);
        
        return CreatedAtRoute("GetNoteById", new { id = noteReadDto.Id.ToString()}, noteReadDto);
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