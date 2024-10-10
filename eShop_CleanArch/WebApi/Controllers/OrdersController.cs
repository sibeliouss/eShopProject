using Application.Features.Orders.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace WebApi.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{customerId}")]
    public async Task<IActionResult> GetAllOrdersByCustomerId(Guid customerId)
    {
        
        var query = new GetAllOrdersByCustomerId { CustomerId = customerId };
        var result = await _mediator.Send(query);
            
        return Ok(result);
    }
    
    [HttpGet("{customerId:guid}")]
    public async Task<IActionResult> GetOrderReceivedByCustomerId(Guid customerId)
    {
        var query = new GetOrderReceivedByCustomerId { CustomerId = customerId };
        var result = await _mediator.Send(query);
            
        return Ok(result);
     
    }
}
