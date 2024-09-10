using Application.Features.Addresses.Commands.Create;
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
        
        CreateAddressCommandValidator informationValidator = new();
        var validationResult = await informationValidator.ValidateAsync(command);

        if (!validationResult.IsValid)
        {
            return StatusCode(422, validationResult.Errors.Select(s => s.ErrorMessage));
        }

        var response= await _mediator.Send(command);
        return Ok(response);
    }

}