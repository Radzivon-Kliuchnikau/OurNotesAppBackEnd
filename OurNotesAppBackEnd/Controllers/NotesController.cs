using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OurNotesAppBackEnd.Dtos.Note;
using OurNotesAppBackEnd.Interfaces;
using OurNotesAppBackEnd.Models;
using OurNotesAppBackEnd.Utils;

namespace OurNotesAppBackEnd.Controllers;

[Authorize]
[Route("api/notes")]
[ApiController]
public class NotesController : ControllerBase
{
    private readonly INoteRepository _noteRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<NotesController> _logger;
    private readonly IGrantAccessToNoteService _grantAccessToNoteService;

    public NotesController(
        INoteRepository noteRepository, 
        IMapper mapper, 
        ILogger<NotesController> logger,
        IGrantAccessToNoteService grantAccessToNoteService)
    {
        _noteRepository = noteRepository;
        _mapper = mapper;
        _logger = logger;
        _grantAccessToNoteService = grantAccessToNoteService;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<NoteReadDto>>> GetAllNotes()
    {
        _logger.LogInformation("Request received by Controller {Controller}, Action: {ControllerAction}", nameof(NotesController), nameof(GetAllNotes));
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return Unauthorized(ErrorMessages.UserIdNotFound);
        }
        
        var notes = await _noteRepository.GetNotesUserHaveAccessTo(userId);
        
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
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return Unauthorized(ErrorMessages.UserIdNotFound);
        }
        
        var noteModel = _mapper.Map<Note>(noteCreateDto);
        noteModel.AppUserId = userId;
        
        await _noteRepository.AddEntityAsync(noteModel);
        await _grantAccessToNoteService.GrantAccessToNoteAsync(noteModel, noteCreateDto.UsersEmailsWithGrantedAccess);

        var noteReadDto = _mapper.Map<NoteReadDto>(noteModel);
        
        return CreatedAtRoute(nameof(GetNoteById), new { id = noteReadDto.Id.ToString()}, noteReadDto);
    }
    
    [HttpPost("{noteId}/grant-access")]
    public async Task<ActionResult> GrantAccessToNote([FromRoute] string noteId, [FromBody] string[] listOfUsersToGrantAccessTo)
    {
        var note = await _noteRepository.GetEntityByIdAsync(Guid.Parse(noteId));
        if (note == null)
        {
            return NotFound(ErrorMessages.NoteNotFound);
        }
        
        await _grantAccessToNoteService.GrantAccessToNoteAsync(note, listOfUsersToGrantAccessTo); // TODO: Think about Error handling of this method

        return Ok(ErrorMessages.AccessGrantedSuccessfully);
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