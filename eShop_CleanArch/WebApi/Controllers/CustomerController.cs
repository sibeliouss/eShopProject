using Application.Features.Customers.Commands.Delete;
using Application.Features.Customers.Commands.Update.UpdateCustomerInformation;
using Application.Features.Customers.Commands.Update.UpdateCustomerPassword;
using Application.Features.Customers.Dtos;
using Application.Features.Customers.Queries;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Controllers.Abstract;

namespace WebApi.Controllers;

public class CustomerController : ApiController
{
    private readonly IMediator _mediator;
    public CustomerController(IMediator mediator)
    {
        _mediator = mediator;
    } 
    
    [HttpPut]
    public async Task<IActionResult> UpdateCustomerInformation([FromBody] UpdateCustomerInformationDto updateCustomerInformationDto , CancellationToken cancellationToken)
    { 
        UpdateCustomerInformationCommandValidator validator = new();
        var validationResult = await validator.ValidateAsync(updateCustomerInformationDto, cancellationToken);

        if (!validationResult.IsValid)
        {
            return StatusCode(422, validationResult.Errors.Select(s => s.ErrorMessage));
        }
        var command = new UpdateCustomerInformationCommand
        {
            UpdateCustomerInformationDto = updateCustomerInformationDto
        };
        
        try
        { 
            await _mediator.Send(command, cancellationToken);
            return Ok("Müşteri bilgileri başarıyla güncellendi.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message); 
        }
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdatePassword([FromBody] UpdateCustomerPasswordDto updateCustomerPasswordDto,CancellationToken cancellationToken)
    {
        UpdateCustomerPasswordCommandValidator validator = new();
        var validationResult = await validator.ValidateAsync(updateCustomerPasswordDto, cancellationToken);

        if (!validationResult.IsValid)
        {
            return StatusCode(422, validationResult.Errors.Select(s => s.ErrorMessage));
        }
        var command = new UpdateCustomerPasswordCommand
        {
            UpdateCustomerPasswordDto = updateCustomerPasswordDto
        };
        try
        {
                await _mediator.Send(command, cancellationToken);
                return Ok("Şifre başarıyla değiştirildi.");
        }
        catch (Exception ex)
        {
                return BadRequest(new { error = ex.Message });
        }
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCustomerById(Guid id)
    {
           var query = new GetCustomerByIdQuery
            {
                CustomerId = id
            };

            var customer = await _mediator.Send(query);
            return Ok(customer); // Müşteri bulunduysa 200 OK döner
        
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomerDto>>> GetAll()
    {
        var query = new GetAllCustomersQuery();
        var customers = await _mediator.Send(query);

        if (!customers.Any())
        {
            return NotFound("No customers found.");
        }

        return Ok(customers);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCustomer(Guid id, [FromBody] string password)
    {
        if (id == Guid.Empty)
        {
            return BadRequest("Geçersiz müşteri ID.");
        }

        try
        {
            var command = new DeleteCustomerCommand
            {
                CustomerId = id,
                Password = password
            };

            await _mediator.Send(command);

            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}



