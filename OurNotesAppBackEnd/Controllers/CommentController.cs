using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OurNotesAppBackEnd.Dtos.Comment;
using OurNotesAppBackEnd.Interfaces;
using OurNotesAppBackEnd.Models;

namespace OurNotesAppBackEnd.Controllers;

[Route("api/comment")]
[ApiController]
public class CommentController : ControllerBase
{
    private readonly ICommentRepository _commentRepository;
    private readonly IMapper _mapper;

    public CommentController(ICommentRepository commentRepository, IMapper mapper)
    {
        _commentRepository = commentRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllComments()
    {
        var comments = await _commentRepository.GetAllEntitiesAsync();

        return Ok(_mapper.Map<IEnumerable<CommentReadDto>>(comments));
    }

    [HttpGet()]
    [Route("{id}")]
    public async Task<IActionResult> GetCommentById([FromRoute] int id)
    {
        var comment = await _commentRepository.GetEntityByIdAsync(id);

        return Ok(_mapper.Map<CommentReadDto>(comment));
    }

    [HttpPost]
    public async Task<IActionResult> AddComment([FromBody] CommentCreateDto commentCreateDto)
    {
        var commentModel = _mapper.Map<Comment>(commentCreateDto);
        await _commentRepository.AddEntityAsync(commentModel);

        return CreatedAtAction(nameof(GetCommentById), new { id = commentModel.Id },
            _mapper.Map<CommentReadDto>(commentModel));
    }

    [HttpPost]
    [Route("{id}")]
    public async Task<IActionResult> UpdateComment([FromRoute] int id, [FromBody] CommentUpdateDto commentUpdateDto)
    {
        var commentModel = await _commentRepository.GetEntityByIdAsync(id);
        if (commentModel == null)
        {
            return NotFound();
        }
        
        // Source -> Destination
        _mapper.Map(commentUpdateDto, commentModel);
        await _commentRepository.EditEntity(commentModel);

        return Ok(_mapper.Map<CommentReadDto>(commentModel));
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> RemoveComment([FromRoute] int id)
    {
        var comment = await _commentRepository.GetEntityByIdAsync(id);

        if (comment == null)
        {
            return NotFound();
        }

        await _commentRepository.DeleteEntity(comment);

        return NoContent();
    }
}