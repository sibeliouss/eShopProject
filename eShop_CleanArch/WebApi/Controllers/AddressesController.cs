using Application.Features.Addresses.Commands.Create;
using Application.Features.Addresses.Commands.Delete;
using Application.Features.Addresses.Commands.Update;
using Application.Features.Addresses.Queries;
using Application.Features.BillingAddresses.Commands.Create;
using Application.Features.BillingAddresses.Commands.Delete;
using Application.Features.BillingAddresses.Commands.Update;
using Application.Features.BillingAddresses.Dtos;
using Application.Features.BillingAddresses.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace WebApi.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
public class AddressesController : ControllerBase
{
    private readonly IMediator _mediator;

    public AddressesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateAddressCommand command)
    {
        var response= await _mediator.Send(command);
        return Ok(response);
    }
    
    [HttpPut]
    public async Task<IActionResult> Update(UpdateAddressCommand command)
    {
        var response = await _mediator.Send(command);
        return Ok(response);
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var command = new DeleteAddressCommand { Id = id };
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> Get(Guid userId)
    {
        var query = new GetListAddressQuery()
        {
            UserId = userId
        };
       var response= await _mediator.Send(query);
       return Ok(response);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateBillingAddress(CreateBillingAddressDto createBillingAddressDto)
    {
        var command = new CreateBillingAddressCommand
        {
            CreateBillingAddressDto = createBillingAddressDto
        };

        var response= await _mediator.Send(command);
        return Ok(response);
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateBillingAddress(UpdateBillingAddressDto updateCustomerPasswordDto)
    {
       
        var command = new  UpdateBillingAddressCommand
        {
            UpdateBillingAddressDto = updateCustomerPasswordDto
        };

        var response = await _mediator.Send(command);
        return Ok(response);
    }
    
    [HttpGet("{userId:guid}")]
    public async Task<IActionResult> GetBillingAddress(Guid userId)
    {
        var query = new GetListBillingAddressQuery()
        {
            UserId = userId
        };
        var response= await _mediator.Send(query);
        return Ok(response);
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteBillingAddress([FromRoute] Guid id)
    {
        var command = new DeleteBillingAddressCommand() { Id = id };
        await _mediator.Send(command);
        return NoContent();
    }
}