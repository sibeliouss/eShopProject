using Application.Features.Customers.Commands.Update.UpdateCustomerInformation;
using Application.Features.Customers.Commands.Update.UpdateCustomerPassword;
using Application.Features.Customers.Dtos;
using FluentValidation.Results;
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
}

