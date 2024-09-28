using Application.Features.Categories.Commands.Create;
using Application.Features.Categories.Commands.Delete;
using Application.Features.Categories.Commands.Update;
using Application.Features.Categories.Dtos;
using Application.Features.Categories.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Controllers.Abstract;

namespace WebApi.Controllers;

public class CategoriesController : ApiController
{
    private readonly IMediator _mediator;

    public CategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCategoryCommand command)
    {
        var response= await _mediator.Send(command);
        return Ok(response);
    }
    
    [HttpPut]
    public async Task<IActionResult> Update(UpdateCategoryCommand command)
    {
        var response = await _mediator.Send(command);
        return Ok(response);
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetListCategoryDto>>> GetAllCategories()
    {
        var query = new GetAllCategoriesQuery();
        var categories = await _mediator.Send(query);

        if (!categories.Any())
        {
            return NotFound("Kategori bulunmadı.");
        }

        return Ok(categories);
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var command = new DeleteCategoryCommand() { Id = id };
        await _mediator.Send(command);
        return NoContent();
    }
}