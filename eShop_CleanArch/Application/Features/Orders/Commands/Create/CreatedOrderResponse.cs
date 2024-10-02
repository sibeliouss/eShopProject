namespace Application.Features.Orders.Commands.Create;

public class CreatedOrderResponse
{
    public Guid Id { get; set; }
    public string OrderNumber { get; set; }
    public DateTime CreateAt { get; set; }
    public int ProductQuantiy { get; set; }
    public string Status { get; set; }
    public string PaymentCurrency { get; set; }
}