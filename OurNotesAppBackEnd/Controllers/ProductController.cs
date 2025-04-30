using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OurNotesAppBackEnd.Dtos.Product;
using OurNotesAppBackEnd.Interfaces;
using OurNotesAppBackEnd.Models;
using OurNotesAppBackEnd.Utils;

namespace OurNotesAppBackEnd.Controllers;

[Route("api/product")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public ProductController(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<ProductReadDto>> GetAllProducts([FromQuery] ProductQueryObject? productQuery)
    {
        var products = await _productRepository.GetAllEntitiesAsync(productQuery);

        return Ok(_mapper.Map<IEnumerable<ProductReadDto>>(products));
    }

    [HttpGet]
    [Route("{id:guid}")]
    public async Task<ActionResult<ProductReadDto>> GetProductById([FromRoute] Guid id)
    {
        var product = await _productRepository.GetEntityByIdAsync(id);

        if (product == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<ProductReadDto>(product));
    }

    [HttpPost]
    public async Task<ActionResult<ProductReadDto>> AddProduct([FromBody] ProductCreateDto productCreateDto)
    {
        var productModel = _mapper.Map<Product>(productCreateDto);
        await _productRepository.AddEntityAsync(productModel);

        return CreatedAtAction(nameof(GetProductById), new { id = productModel.Id }, _mapper.Map<ProductReadDto>(productModel));
    }

    [HttpPut]
    [Route("{id:guid}")]
    public async Task<IActionResult> UpdateProduct([FromRoute] Guid id, [FromBody] ProductUpdateDto productUpdateDto)
    {
        var productModel = await _productRepository.GetEntityByIdAsync(id);
        if (productModel == null)
        {
            return NotFound();
        }
        
        // Source => Destination
        _mapper.Map(productUpdateDto, productModel);
        await _productRepository.EditEntity(productModel);
        
        return Ok(_mapper.Map<ProductReadDto>(productModel));
    }

    [HttpDelete]
    [Route("{id:guid}")]
    public async Task<IActionResult> RemoveProduct([FromRoute] Guid id)
    {
        var product = await _productRepository.GetEntityByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        
        await _productRepository.DeleteEntity(product);
        
        return NoContent();
    }
}