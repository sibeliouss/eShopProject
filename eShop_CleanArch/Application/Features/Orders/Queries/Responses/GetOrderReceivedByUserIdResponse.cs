using Application.Features.Orders.Dtos;

namespace Application.Features.Orders.Queries.Responses;

public class GetOrderReceivedByUserIdResponse
{
    public Guid Id { get; set; }
    public string OrderNumber { get; set; }
    public int ProductQuantity { get; set; }
    public DateTime CreateAt { get; set; }
    public string PaymentMethod { get; set; }
    public string Status { get; set; }
    public string PaymentNumber { get; set; }
    public string PaymentCurrency { get; set; }
    public IEnumerable<OrderDetailDto> Products { get; set; } = new List<OrderDetailDto>();
}