using Application.Features.Auth.Login;
using Application.Features.Auth.Register;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Controllers.Abstract;

namespace WebApi.Controllers;
[AllowAnonymous]
public class AuthController : ApiController
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _mediator.Send(command, cancellationToken);
            return Ok(new { Info = response });
            /*return Created(uri: "", response);*/
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }
    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegisterCommand command, CancellationToken cancellationToken)
    {

        try
        {
            await _mediator.Send(command, cancellationToken);
            return Ok("Kullanıcı başarıyla oluşturuldu.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

}

