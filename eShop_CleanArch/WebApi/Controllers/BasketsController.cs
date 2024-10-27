using Application.Features.Baskets.Commands.Create;
using Application.Features.Baskets.Commands.Delete;
using Application.Features.Baskets.Commands.Payment;
using Application.Features.Baskets.Dtos;
using Application.Features.Baskets.Queries;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace WebApi.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class BasketsController : ControllerBase
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
    
    [HttpPost]
    public async Task<IActionResult> Payment([FromBody] PaymentDto paymentDto)
    {
        var command = new PaymentCommand
        {
            PaymentDto = paymentDto
        };

        try
        {
            await _mediator.Send(command);
            return Ok(new { Message = "Payment successful." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }
}
