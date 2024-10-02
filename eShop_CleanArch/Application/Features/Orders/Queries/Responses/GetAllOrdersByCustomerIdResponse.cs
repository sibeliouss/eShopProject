namespace Application.Features.Orders.Queries.Responses;

public class GetAllOrdersByCustomerIdResponse
{
    public Guid Id { get; set; }
    public string OrderNumber { get; set; }
    public DateTime CreateAt { get; set; }
    public int ProductQuantity { get; set; }
    public string Status { get; set; }
    public string PaymentCurrency { get; set; }
}