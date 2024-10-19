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

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetAllOrdersByUserId(Guid userId)
    {
        
        var query = new GetAllOrdersByUserId { UserId = userId };
        var result = await _mediator.Send(query);
            
        return Ok(result);
    }
    
    [HttpGet("{userId:guid}")]
    public async Task<IActionResult> GetOrderReceivedByUserrId(Guid userId)
    {
        var query = new GetOrderReceivedByCustomerId { UserId = userId };
        var result = await _mediator.Send(query);
            
        return Ok(result);
     
    }
}
