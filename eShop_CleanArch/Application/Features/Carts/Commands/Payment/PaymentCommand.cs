using System.Globalization;
using Application.Features.Carts.Dtos;
using Application.Services.Orders;
using Application.Services.Products;
using Application.Services.Repositories;
using Application.Services.Users;
using Domain.Entities;
using Domain.Entities.ValueObjects;
using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Carts.Commands.Payment;

public class PaymentCommand : IRequest
{
    public PaymentDto PaymentDto { get; set; } 
}

public class PaymentCommandHandler : IRequestHandler<PaymentCommand>
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductService _productService;
    private readonly IUserService _userService;
    private readonly IOrderService _orderService;

    public PaymentCommandHandler(ICartRepository cartRepository, IProductService productService, IUserService userService, IOrderService orderService)
    {
        _cartRepository = cartRepository;
        _productService = productService;
        _userService = userService;
        _orderService = orderService;
    }

    public async Task Handle(PaymentCommand request, CancellationToken cancellationToken)
    {
        var paymentRequest = request.PaymentDto; 
        double total = 0; 
        var shippingAndCartTotal = Convert.ToDouble(paymentRequest.ShippingAndCartTotal);

        // Ürün stoğu kontrolü
        foreach (var item in paymentRequest.Products)
        {
            var product = await _productService.Query()
                .AsNoTracking()  // Takip etmeyin
                .FirstOrDefaultAsync(p => p.Id == item.Id);

            if (product == null || item.Quantity > product.Quantity)
            {
                throw new Exception("Ürün stoğu yetersiz. Lütfen daha az ürün ile tekrar deneyin.");
            }
        }

        // Toplam tutar hesaplaması
        foreach (var product in paymentRequest.Products)
        {
            total += Convert.ToDouble(product.Price.Value);
        }

        // Iyzipay ödeme işlemi
        var options = new Options
        {
            ApiKey = "sandbox-eD7ko3qqCk1xnw4TXD7GGwZqaOXRFQ4m",
            SecretKey = "sandbox-yjOKSl0aC3EeVmnN0XJCgHvGWM46Sx8W",
            BaseUrl = "https://sandbox-api.iyzipay.com"
        };

        var requestPayment = new CreatePaymentRequest();
        requestPayment.Locale = Locale.TR.ToString();
        requestPayment.ConversationId = Guid.NewGuid().ToString();
        requestPayment.Price = total.ToString("F2", CultureInfo.InvariantCulture); // Ürünlerin toplam fiyatı
        requestPayment.PaidPrice = shippingAndCartTotal.ToString("F2", CultureInfo.InvariantCulture); // Ödenen toplam tutar (ürünler + kargo)
        requestPayment.Currency = "TRY"; 
        requestPayment.Installment = 1; // Taksit sayısı (1: Tek çekim)
        requestPayment.BasketId = _orderService.GetNewOrderNumber(); 
        requestPayment.PaymentChannel = PaymentChannel.WEB.ToString(); 
        requestPayment.PaymentGroup = PaymentGroup.PRODUCT.ToString(); 

        // Müşteri tarafından girilen ödeme kartı bilgileri
        requestPayment.PaymentCard = paymentRequest.PaymentCard; 

        // Alıcı bilgileri
        Buyer buyer = paymentRequest.Buyer;
        buyer.Id = Guid.NewGuid().ToString(); // buyerId'yi burada ekliyoruz
        requestPayment.Buyer = buyer; // Buyer bilgilerini ödeme talebine ekliyoruz


        requestPayment.ShippingAddress = paymentRequest.Address; 
        requestPayment.BillingAddress = paymentRequest.BillingAddress;

        // Sepet ürünleri
        var basketItems = paymentRequest.Products.Select(product => new BasketItem
        {
            Category1 = "Giyim",
            Category2 = "Ev & Dekorasyon",
            Id = product.Id.ToString(),
            Name = product.Name,
            ItemType = BasketItemType.PHYSICAL.ToString(),
            Price = product.Price.Value.ToString("F2", CultureInfo.InvariantCulture)
        }).ToList();

        requestPayment.BasketItems = basketItems;

        var payment = Iyzipay.Model.Payment.Create(requestPayment, options);

        if (payment.Status == "success")
        {
            foreach (var item in paymentRequest.Products)
            {
                var product = await _productService.FindAsync(item.Id);
                if (product != null && product.Quantity >= item.Quantity)
                {
                    product.Quantity -= item.Quantity;
                }

                if (product != null) await _productService.UpdateAsync(product);
            }

            var orderNumber = _orderService.GetNewOrderNumber();
            var totalProductQuantity = paymentRequest.Products.Sum(p => p.Quantity);

            var order = new Order
            {
                OrderNumber = orderNumber,
                UserId = paymentRequest.UserId,
                PaymentDate = DateTime.Now,
                PaymentMethod = "Kredi Kartı",
                PaymentNumber = payment.PaymentId,
                Status = "BeingPrepared",
                PaymentCurrency = "TRY",
                ProductQuantity = totalProductQuantity,
                CreateAt = DateTime.Now,
                OrderDetails = paymentRequest.Products.Select(p => new OrderDetail
                {
                    ProductId = p.Id,
                    Quantity = p.Quantity,
                    Price = new Money(p.Price.Value, p.Price.Currency)
                }).ToList()
            };

            await _orderService.AddAsync(order);

            var user = await _userService.FindAsync(paymentRequest.UserId);
            if (user is not null)
            {
                var baskets = await _cartRepository.Query()
                    .Where(p => p.UserId == paymentRequest.UserId)
                    .ToListAsync(cancellationToken);
                await _cartRepository.DeleteRangeAsync(baskets);
            }
        }
        else
        {
            throw new Exception(payment.ErrorMessage);
        }
    }
}
