using Domain.Entities;
using Iyzipay.Model;

namespace Application.Features.Baskets.Dtos;

public record PaymentDto(
    Guid UserId,
    List<Product> Products,
    Buyer Buyer,
    Iyzipay.Model.Address Address,
    Iyzipay.Model.Address BillingAddress,
    PaymentCard PaymentCard,
    decimal ShippingAndCartTotal,
    string Currency );