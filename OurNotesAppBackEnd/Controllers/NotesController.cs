using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
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
    private readonly ILogger<NotesController> _logger;

    public NotesController(INotesService notesService, IMapper mapper, ILogger<NotesController> logger)
    {
        _notesService = notesService;
        _mapper = mapper;
        _logger = logger;
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<NoteReadDto>))]
    public async Task<ActionResult<IEnumerable<NoteReadDto>>> GetAllNotes()
    {
        _logger.LogInformation("Request received by Controller {Controller}, Action: {ControllerAction}", nameof(NotesController), nameof(GetAllNotes));
        var notes = await _notesService.GetAllNotes();
        
        return Ok(_mapper.Map<IEnumerable<NoteReadDto>>(notes));
    }

    [HttpGet("{id}", Name = "GetNoteById")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(NoteReadDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<NoteReadDto>> GetNoteById([FromRoute] string id)
    {
        var note = await _notesService.GetNoteById(id);

        if (note == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<NoteReadDto>(note));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(NoteReadDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<NoteReadDto>> CreateNote([FromBody] NoteCreateDto noteCreateDto)
    {
        var noteModel = _mapper.Map<Note>(noteCreateDto);
        await _notesService.AddNote(noteModel);

        var noteReadDto = _mapper.Map<NoteReadDto>(noteModel);
        
        return CreatedAtRoute("GetNoteById", new { id = noteReadDto.Id.ToString()}, noteReadDto);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateNote([FromRoute] string id, [FromBody] NoteUpdateDto noteUpdateDto)
    {
        var noteForUpdateModel = await _notesService.GetNoteById(id);
        if (noteForUpdateModel == null)
        {
            return NotFound();
        }

        _mapper.Map(noteUpdateDto, noteForUpdateModel);
        await _notesService.EditNote(noteForUpdateModel);

        return NoContent();
    }

    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> PartialNoteUpdate([FromRoute] string id, JsonPatchDocument<NoteUpdateDto> patchDocument)
    {
        var noteForUpdateModel = await _notesService.GetNoteById(id);
        if (noteForUpdateModel == null)
        {
            return NotFound();
        }

        var noteToPatch = _mapper.Map<NoteUpdateDto>(noteForUpdateModel);
        patchDocument.ApplyTo(noteToPatch, ModelState);
        if (!TryValidateModel(noteToPatch))
        {
            return ValidationProblem(ModelState);
        }

        _mapper.Map(noteToPatch, noteForUpdateModel);
        await _notesService.EditNote(noteForUpdateModel);

        return NoContent();
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteNote([FromRoute] string id)
    {
        var noteForRemoveModel = await _notesService.GetNoteById(id);
        if (noteForRemoveModel == null)
        {
            return NotFound();
        }
        
        await _notesService.DeleteNote(noteForRemoveModel);

        return NoContent();
    }
}