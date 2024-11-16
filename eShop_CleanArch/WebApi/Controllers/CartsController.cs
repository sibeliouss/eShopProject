
using Application.Features.Carts.Commands.Create;
using Application.Features.Carts.Commands.Delete;
using Application.Features.Carts.Commands.Payment;
using Application.Features.Carts.Dtos;
using Application.Features.Carts.Queries;
using Application.Features.Carts.Queries.Responses;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace WebApi.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class CartsController : ControllerBase
{
    private readonly IMediator _mediator;

    public CartsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateCart([FromBody] ShoppingCartDto shoppingCartDto)
    {
        var command = new CreateShoppingCartCommand
        {
            ShoppingCartDto = shoppingCartDto
        };
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(CreateCart), new { id = result.ProductId }, result);
    }

    [HttpGet("{productId:guid}/{quantity:int}")]
    public async Task<IActionResult> CheckProductQuantityIsAvailable(Guid productId, int quantity)
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
    public async Task<IActionResult> DeleteCart(Guid id)
    {
        var command = new DeleteCartCommand { Id = id };
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

   
    
    [HttpGet("{productId:guid}/{quantity:int}")]
    public async Task<IActionResult> ChangeProductQuantityInCart(Guid productId, int quantity)
    {
        try
        {
            
            var command = new ChangeProductQuantityInCart(productId, quantity);
            await _mediator.Send(command);
            return NoContent();
        }
        catch (Exception ex)
        {
            // Hata mesajı döndürüyoruz
            return StatusCode(422, new { message = ex.Message });
        }
    }

    [HttpGet("{userId:guid}")]
    public async Task<ActionResult<List<CartResponse>>> GetAllCart(Guid userId)
    {
        var query = new GetAllCartQuery { UserId = userId };
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}