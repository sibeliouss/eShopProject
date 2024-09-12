namespace Application.Features.Addresses.Queries;

public class GetAddressQueryResponse
{
   public Guid Id { get; set; }
   public Guid CustomerId { get; set; }
   public string Country { get; set; }
   public string City { get; set; }
   public string? ZipCode { get; set; }
   public string ContactName { get; set; }
   public string Description { get; set; } 
}