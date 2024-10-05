using Application.Features.Baskets.Commands.Create;
using Application.Features.Baskets.Commands.Delete;
using Application.Features.Baskets.Dtos;
using Application.Features.Baskets.Queries;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Controllers.Abstract;

namespace WebApi.Controllers;

public class BasketsController : ApiController
{
    private readonly IMediator _mediator;

    public BasketsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateBasket([FromBody] ShoppingBasketDto shoppingBasketDto)
    {

        var command = new CreateShoppingBasketCommand
        {
            ShoppingBasketDto = shoppingBasketDto
        };
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(CreateBasket), new { id = result.ProductId }, result);
    }
    
    
    [HttpGet("{productId:guid}/{quantity:int}")]
    public async Task<IActionResult> CheckProductQuantity(Guid productId, int quantity)
    {
        var query = new GetCheckProductQuantityIsAvailableQuery
        {
            ProductId = productId,
            Quantity = quantity
        };
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteBasket(Guid id)
    {
        var command = new DeleteBasketCommand { Id = id };
        await _mediator.Send(command);
        return NoContent();
    }
}
