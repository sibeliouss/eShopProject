using Application.Features.Addresses.Commands.Create;
using Application.Features.Addresses.Commands.Delete;
using Application.Features.Addresses.Commands.Update;
using Application.Features.Addresses.Queries;
using Application.Features.BillingAddresses.Commands.Create;
using Application.Features.BillingAddresses.Commands.Delete;
using Application.Features.BillingAddresses.Commands.Update;
using Application.Features.BillingAddresses.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Controllers.Abstract;

namespace WebApi.Controllers;

public class AddressesController : ApiController
{
    private readonly IMediator _mediator;

    public AddressesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateAddressCommand command)
    {
        var validationResult = await new CreateAddressCommandValidator().ValidateAsync(command);
        if (!validationResult.IsValid)
        {
            return StatusCode(422, validationResult.Errors.Select(s => s.ErrorMessage));
        }

        var response= await _mediator.Send(command);
        return Ok(response);
    }
    
    [HttpPut]
    public async Task<IActionResult> Update(UpdateAddressCommand command)
    {
        var validationResult = await new UpdateAddressCommandValidator().ValidateAsync(command);
        if (!validationResult.IsValid)
        {
            return StatusCode(422, validationResult.Errors.Select(s => s.ErrorMessage));
        }

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

    [HttpGet("{customerId}")]
    public async Task<IActionResult> Get(Guid customerId)
    {
        var query = new GetListAddressQuery()
        {
            CustomerId = customerId
        };
       var response= await _mediator.Send(query);
       return Ok(response);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateBillingAddress(CreateBillingAddressDto createBillingAddressDto)
    {
        var validationResult = await new CreateBillingAddressCommandValidator().ValidateAsync(createBillingAddressDto);
        if (!validationResult.IsValid)
        {
            return StatusCode(422, validationResult.Errors.Select(s => s.ErrorMessage));
        }
        
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
        var validationResult = await new UpdateBillingAddressCommandValidator().ValidateAsync(updateCustomerPasswordDto);
        if (!validationResult.IsValid)
        {
            return StatusCode(422, validationResult.Errors.Select(s => s.ErrorMessage));
        }
        var command = new  UpdateBillingAddressCommand
        {
            UpdateBillingAddressDto = updateCustomerPasswordDto
        };

        var response = await _mediator.Send(command);
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