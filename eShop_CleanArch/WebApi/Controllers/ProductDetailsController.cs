using Application.Features.ProductDetails.Commands.Create;
using Application.Features.ProductDetails.Commands.Delete;
using Application.Features.ProductDetails.Commands.Update;
using Application.Features.ProductDetails.Dtos;
using Application.Features.ProductDetails.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Controllers.Abstract;

namespace WebApi.Controllers;

public class ProductDetailsController : ApiController
{
    private readonly IMediator _mediator;

    public ProductDetailsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPost]
    public async Task<IActionResult> CreateProductDetail([FromBody] CreateProductDetailDto createProductDetailDto)
    {
        

        var command = new CreateProductDetailCommand
        {
            CreateProductDetailDto = createProductDetailDto
        };

        var result = await _mediator.Send(command);

        return Ok(result); 
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateProductDetail([FromBody] UpdateProductDetailDto updateProductDetailDto)
    {
        var command = new UpdateProductDetailCommand
        {
            UpdateProductDetailDto = updateProductDetailDto
        };
        var updatedProductDetailResponse = await _mediator.Send(command);
        return Ok(updatedProductDetailResponse);
    }
    
    [HttpGet]
    public async Task<ActionResult<List<GetAllProductDetailResponse>>> GetAllProductDetails()
    {
        var query = new GetAllProductDetailQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var command = new DeleteProductDetailCommand() { Id = id };
        await _mediator.Send(command);
        return NoContent();
    }
}