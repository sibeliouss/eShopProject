using Application.Features.Customers.Commands.Create;
using Application.Features.Customers.Commands.Delete;
using Application.Features.Customers.Commands.Update.UpdateCustomerInformation;
using Application.Features.Customers.Commands.Update.UpdateCustomerPassword;
using Application.Features.Customers.Dtos;
using Application.Features.Customers.Queries;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace WebApi.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
public class CustomersController : ControllerBase
{
    private readonly IMediator _mediator;
    public CustomersController(IMediator mediator)
    {
        _mediator = mediator;
    } 
    
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateCustomerCommand createCustomerCommand)
    {

        var response = await _mediator.Send(createCustomerCommand);

        // Yeni oluşturulan müşteri için URI'yi oluşturun (örneğin, customer id'ye göre)
       
        return Created(uri: "", response);
    }

    
    [HttpPut]
    public async Task<IActionResult> UpdateCustomerInformation([FromBody] UpdateCustomerInformationDto updateCustomerInformationDto)
    { 
        UpdateCustomerInformationCommandValidator validator = new();
        var validationResult = await validator.ValidateAsync(updateCustomerInformationDto);

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
            await _mediator.Send(command);
            return Ok("Müşteri bilgileri başarıyla güncellendi.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message); 
        }
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateCustomerPassword([FromBody] UpdateCustomerPasswordDto updateCustomerPasswordDto)
    {
        UpdateCustomerPasswordCommandValidator validator = new();
        var validationResult = await validator.ValidateAsync(updateCustomerPasswordDto);

        if (!validationResult.IsValid)
        {
            return StatusCode(422, validationResult.Errors.Select(s => s.ErrorMessage));
        }
        var command = new UpdateCustomerPasswordCommand
        {
            UpdateCustomerPasswordDto = updateCustomerPasswordDto
        };
       
        await _mediator.Send(command);
        return Ok("Şifre başarıyla değiştirildi.");
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCustomerById(Guid id)
    {
           var query = new GetCustomerByIdQuery
            {
                CustomerId = id
            };

            var customer = await _mediator.Send(query);
            return Ok(customer); 
        
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomerDto>>> GetAll()
    {
        var query = new GetAllCustomersQuery();
        var customers = await _mediator.Send(query);

        if (!customers.Any())
        {
            return NotFound("Müşteri bulunmadı.");
        }

        return Ok(customers);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCustomer(Guid id, [FromQuery] string password)
    {
        
            var command = new DeleteCustomerCommand
            {
                
                CustomerId = id,
                Password = password
            };

            await _mediator.Send(command);

            return NoContent();
    }
}



