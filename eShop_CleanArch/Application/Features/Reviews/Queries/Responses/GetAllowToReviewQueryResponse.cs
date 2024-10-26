using MediatR;

namespace Application.Features.Reviews.Queries.Responses;

public class GetAllowToReviewQueryResponse
{
    public bool IsAllow { get; set; }
    public string Message { get; set; }
}