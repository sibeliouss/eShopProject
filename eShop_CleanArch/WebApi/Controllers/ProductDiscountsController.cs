using Application.Features.ProductDiscounts.Commands.Create;
using Application.Features.ProductDiscounts.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Controllers.Abstract;

namespace WebApi.Controllers;

public class ProductDiscountsController : ApiController
{
    private readonly IMediator _mediator;

    public ProductDiscountsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateProductDiscount([FromBody] ProductDiscountDto productDiscountDto)
    {

        var command = new CreateProductDiscountCommand { ProductDiscountDto = productDiscountDto };
        var response = await _mediator.Send(command);

        return CreatedAtAction(nameof(CreateProductDiscount), new { id = response.Id }, response);
    }
}
