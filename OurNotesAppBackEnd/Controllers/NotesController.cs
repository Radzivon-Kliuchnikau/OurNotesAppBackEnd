using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using OurNotesAppBackEnd.Dtos;
using OurNotesAppBackEnd.Dtos.Note;
using OurNotesAppBackEnd.Interfaces;
using OurNotesAppBackEnd.Models;

namespace OurNotesAppBackEnd.Controllers;

// [Authorize]
[Route("api/notes")]
[ApiController]
public class NotesController : ControllerBase
{
    private readonly INoteRepository _noteRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<NotesController> _logger;

    public NotesController(INoteRepository noteRepository, IMapper mapper, ILogger<NotesController> logger)
    {
        _noteRepository = noteRepository;
        _mapper = mapper;
        _logger = logger;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<NoteReadDto>>> GetAllNotes()
    {
        _logger.LogInformation("Request received by Controller {Controller}, Action: {ControllerAction}", nameof(NotesController), nameof(GetAllNotes));
        var notes = await _noteRepository.GetAllEntitiesAsync();
        
        return Ok(_mapper.Map<IEnumerable<NoteReadDto>>(notes));
    }

    [HttpGet("{id}", Name = "GetNoteById")]
    public async Task<ActionResult<NoteReadDto>> GetNoteById([FromRoute] Guid id)
    {
        var note = await _noteRepository.GetEntityByIdAsync(id);

        if (note == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<NoteReadDto>(note));
    }

    [HttpPost]
    public async Task<ActionResult<NoteReadDto>> CreateNote([FromBody] NoteCreateDto noteCreateDto)
    {
        var noteModel = _mapper.Map<Note>(noteCreateDto);
        await _noteRepository.AddEntityAsync(noteModel);

        var noteReadDto = _mapper.Map<NoteReadDto>(noteModel);
        
        return CreatedAtRoute("GetNoteById", new { id = noteReadDto.Id.ToString()}, noteReadDto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<NoteReadDto>> UpdateNote([FromRoute] Guid id, [FromBody] NoteUpdateDto noteUpdateDto)
    {
        var noteForUpdateModel = await _noteRepository.GetEntityByIdAsync(id);
        if (noteForUpdateModel == null)
        {
            return NotFound();
        }

        _mapper.Map(noteUpdateDto, noteForUpdateModel);
        await _noteRepository.EditEntity(noteForUpdateModel);

        return Ok(_mapper.Map<NoteReadDto>(noteForUpdateModel));
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> PartialNoteUpdate([FromRoute] Guid id, JsonPatchDocument<NoteUpdateDto> patchDocument)
    {
        var noteForUpdateModel = await _noteRepository.GetEntityByIdAsync(id);
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
        await _noteRepository.EditEntity(noteForUpdateModel);

        return NoContent();
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteNote([FromRoute] Guid id)
    {
        var noteForRemoveModel = await _noteRepository.GetEntityByIdAsync(id);
        if (noteForRemoveModel == null)
        {
            return NotFound();
        }
        
        await _noteRepository.DeleteEntity(noteForRemoveModel);

        return NoContent();
    }
}