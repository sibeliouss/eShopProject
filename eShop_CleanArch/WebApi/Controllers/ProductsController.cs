using Application.Features.Products.Commands.Create;
using Application.Features.Products.Commands.Delete;
using Application.Features.Products.Commands.Update;
using Application.Features.Products.Dtos;
using Application.Features.Products.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Controllers.Abstract;

namespace WebApi.Controllers;

public class ProductsController : ApiController
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto createProductDto)
    {
        var command = new CreateProductCommand
        {
            CreateProductDto = createProductDto
        };

        try
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "Ürün oluşturulurken bir hata meydana geldi.", Details = ex.Message });
        }
    }
    [HttpPut]
    public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductDto updateProductDto)
    {
        var command = new UpdateProductCommand
        {
            UpdateProductDto = updateProductDto
        };

        try
        {
            var updatedProductResponse = await _mediator.Send(command);
            return Ok(updatedProductResponse); // Başarılı güncelleme yanıtı
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "Ürün güncellenirken bir hata oluştu.", Details = ex.Message });
        }
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductDetailById(Guid id)
    {
        var query = new GetProductDetailByIdQuery { Id = id };
        var product = await _mediator.Send(query);
        return Ok(product);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllProducts([FromQuery] RequestDto requestDto)
    {
        try
        {
            var query = new GetListProductQuery(requestDto);
            var productsResponse = await _mediator.Send(query);
            return Ok(productsResponse); 
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }
    
    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var products = await _mediator.Send(new GetProductsQuery());
        return Ok(products);
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var command = new DeleteProductCommand() { Id = id };
        await _mediator.Send(command);
        return NoContent();
    }

}