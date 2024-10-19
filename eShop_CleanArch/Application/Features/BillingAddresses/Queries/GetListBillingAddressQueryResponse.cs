namespace Application.Features.BillingAddresses.Queries;

public class GetListBillingAddressQueryResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public string? ZipCode { get; set; }
    public string ContactName { get; set; }
    public string Description { get; set; } 
}