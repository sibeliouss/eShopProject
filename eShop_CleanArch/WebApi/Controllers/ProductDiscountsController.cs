using Application.Features.ProductDiscounts.Commands.Create;
using Application.Features.ProductDiscounts.Commands.Delete;
using Application.Features.ProductDiscounts.Commands.Update;
using Application.Features.ProductDiscounts.Dtos;
using Application.Features.ProductDiscounts.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace WebApi.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
public class ProductDiscountsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductDiscountsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateProductDiscount([FromBody] CreateProductDiscountDto createProductDiscountDto)
    {

        var command = new CreateProductDiscountCommand { CreateProductDiscountDto = createProductDiscountDto };
        var response = await _mediator.Send(command);

        return CreatedAtAction(nameof(CreateProductDiscount), new { id = response.Id }, response);
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateProductDiscount([FromBody] UpdateProductDiscountDto updateProductDiscountDto)
    {
        var command = new UpdateProductDiscountCommand
        {
            UpdateProductDiscountDto = updateProductDiscountDto
        };
        var updatedProductDiscountResponse = await _mediator.Send(command);
        return Ok(updatedProductDiscountResponse);
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var command = new DeleteProductDiscountCommand() { Id = id };
        await _mediator.Send(command);
        return NoContent();
    }
    
    [HttpGet("{productId:guid}")]
    public async Task<IActionResult> GetProductDiscountById(Guid productId)
    {
        var query = new GetProductDiscountByIdQuery(productId);
        var result = await _mediator.Send(query);
        return Ok(result);  
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllProductDiscounts()
    {
        var query = new GetProductDiscountsQuery();
        var result = await _mediator.Send(query);
        

        return Ok(result);  
    }
}
