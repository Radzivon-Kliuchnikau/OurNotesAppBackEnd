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
    private readonly IBaseRepository<Comment, Guid> _commentRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public CommentController(ICommentRepository commentRepository, IProductRepository productRepository, IMapper mapper)
    {
        _commentRepository = commentRepository;
        _productRepository = productRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CommentReadDto>>> GetAllComments()
    {
        var comments = await _commentRepository.GetAllEntitiesAsync();

        return Ok(_mapper.Map<IEnumerable<CommentReadDto>>(comments));
    }

    [HttpGet]
    [Route("{id:guid}")]
    public async Task<ActionResult<CommentReadDto>> GetCommentById([FromRoute] Guid id)
    {
        var comment = await _commentRepository.GetEntityByIdAsync(id);

        return Ok(_mapper.Map<CommentReadDto>(comment));
    }

    [HttpPost]
    [Route("{productId:guid}")]
    public async Task<ActionResult<CommentReadDto>> AddComment([FromRoute] Guid productId, [FromBody] CommentCreateDto commentCreateDto)
    {
        if (await _productRepository.GetEntityByIdAsync(productId) == null)
        {
            return BadRequest("Product does not exist");
        }
        
        var commentModel = _mapper.Map<Comment>(commentCreateDto);
        commentModel.ProductId = productId;
        await _commentRepository.AddEntityAsync(commentModel);

        return CreatedAtAction(nameof(GetCommentById), new { id = commentModel.Id }, _mapper.Map<CommentReadDto>(commentModel));
    }

    [HttpPut]
    [Route("{id:guid}")]
    public async Task<IActionResult> UpdateComment([FromRoute] Guid id, [FromBody] CommentUpdateDto commentUpdateDto)
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
    [Route("{id:guid}")]
    public async Task<IActionResult> RemoveComment([FromRoute] Guid id)
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