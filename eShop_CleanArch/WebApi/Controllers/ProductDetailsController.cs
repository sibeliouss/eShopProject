using Application.Features.ProductDetails.Commands.Create;
using Application.Features.ProductDetails.Dtos;
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
}