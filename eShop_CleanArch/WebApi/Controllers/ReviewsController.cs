using Application.Features.Reviews.Commands.Create;
using Application.Features.Reviews.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class ReviewsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReviewsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateReview(CreateReviewCommand command)
    {
        await _mediator.Send(command);
        return Ok(new { message = "Yorum başarıyla oluşturuldu." });
    }
    
    [HttpGet("{productId:guid}/{userId:guid}")]
    public async Task<IActionResult> AllowToComment(Guid productId, Guid userId)
    {
        var query = new GetAllowToReviewQuery(productId, userId);
        var response = await _mediator.Send(query);

        return Ok(response);
    }
    
    [HttpGet("{productId:guid}")]
    public async Task<IActionResult> GetReviews(Guid productId)
    {
        var query = new GetAllReviewsQuery { ProductId = productId };
        var reviews = await _mediator.Send(query); // Bu artık List<GetAllReviewsQueryResponse> türünde

        return Ok(reviews); 
    }

}