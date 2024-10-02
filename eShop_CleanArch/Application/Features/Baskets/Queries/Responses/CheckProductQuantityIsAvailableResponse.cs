namespace Application.Features.Baskets.Queries.Responses;

public class CheckProductQuantityIsAvailableResponse
{
    public bool IsAvailable { get; set; }
    public string Message { get; set; }
}