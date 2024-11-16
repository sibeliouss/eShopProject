namespace Application.Features.Carts.Queries.Responses;

public class CheckProductQuantityIsAvailableResponse
{
    public bool IsAvailable { get; set; }
    public string Message { get; set; }
}