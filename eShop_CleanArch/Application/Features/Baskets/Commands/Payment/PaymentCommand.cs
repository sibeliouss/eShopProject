using System.Globalization;
using Application.Features.Baskets.Dtos;

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

namespace Application.Features.Baskets.Commands.Payment;

public class PaymentCommand : IRequest
{
    public PaymentDto PaymentDto { get; set; } 
}

public class PaymentCommandHandler : IRequestHandler<PaymentCommand>
{
    private readonly IBasketRepository _basketRepository;
    private readonly IProductService _productService;
    private readonly IUserService _userService;
    private readonly IOrderService _orderService;

    public PaymentCommandHandler(IBasketRepository basketRepository, IProductService productService, IUserService userService, IOrderService orderService)
    {
        _basketRepository = basketRepository;
        _productService = productService;
        _userService = userService;
        _orderService = orderService;
    }
    
    public async Task Handle(PaymentCommand request, CancellationToken cancellationToken)
    {
        var paymentRequest = request.PaymentDto; 
        var currency = ""; 
        double total = 0; 
        var shippingAndCartTotal = Convert.ToDouble(paymentRequest.ShippingAndBasketTotal); // Sepet ve kargo toplamı
        
        foreach (var item in paymentRequest.Products)
        {
            var checkProduct = await _productService.GetByIdAsync(item.Id); 
            if (checkProduct is not null)
            {
                if (item.Quantity > checkProduct.Quantity) 
                    throw new Exception("ürün stoğu yetersiz. lütfen daha az ürün ile tekrar deneyin");
            }
        }
        
        foreach (var product in paymentRequest.Products)
        {
            total += Convert.ToDouble(product.Price.Value); // Her ürünün fiyatı toplam fiyata ekleniyor
        }
        
        if (paymentRequest.Currency == "₺")
        {
            currency = "TRY";
        }

        // Iyzipay
        var options = new Options();
        options.ApiKey = "sandbox-eD7ko3qqCk1xnw4TXD7GGwZqaOXRFQ4m";
        options.SecretKey = "sandbox-yjOKSl0aC3EeVmnN0XJCgHvGWM46Sx8W"; 
        options.BaseUrl = "https://sandbox-api.iyzipay.com"; 

        // Ödeme talebi
        var requestPayment = new CreatePaymentRequest();
        requestPayment.Locale = Locale.TR.ToString(); 
        requestPayment.ConversationId = Guid.NewGuid().ToString();
        requestPayment.Price = total.ToString("F2", CultureInfo.InvariantCulture); // Ürünlerin toplam fiyatı
        requestPayment.PaidPrice = shippingAndCartTotal.ToString("F2", CultureInfo.InvariantCulture); // Ödenen toplam tutar (ürünler + kargo)
        requestPayment.Currency = currency; 
        requestPayment.Installment = 1; // Taksit sayısı (1: Tek çekim)
        requestPayment.BasketId = _orderService.GetNewOrderNumber(); 
        requestPayment.PaymentChannel = PaymentChannel.WEB.ToString(); 
        requestPayment.PaymentGroup = PaymentGroup.PRODUCT.ToString(); 
        
        requestPayment.PaymentCard = paymentRequest.PaymentCard; // Müşteri tarafından girilen ödeme kartı bilgileri

        // Alıcı bilgileri
        Buyer buyer = paymentRequest.Buyer;
        buyer.Id = Guid.NewGuid().ToString(); 
        requestPayment.Buyer = paymentRequest.Buyer;
        
        requestPayment.ShippingAddress = paymentRequest.Address; 
        requestPayment.BillingAddress = paymentRequest.BillingAddress;

        var basketItems = new List<BasketItem>();
        foreach (var product in paymentRequest.Products)
        {
            var item = new BasketItem();
            item.Category1 = "Giyim";
            item.Category2 = "Giyim";
            item.Id = product.Id.ToString();
            item.Name = product.Name;
            item.ItemType = BasketItemType.PHYSICAL.ToString();
            item.Price = product.Price.Value.ToString("F2", CultureInfo.InvariantCulture);
            basketItems.Add(item);

        }

        requestPayment.BasketItems = basketItems;

        var payment = Iyzipay.Model.Payment.Create(requestPayment, options);
        Console.WriteLine(payment);

        if (payment.Status == "success")
        {
            foreach (var item in paymentRequest.Products)
            {
                var product = await _productService.GetByIdAsync(item.Id);
                if (product is not null && item.Quantity <= product.Quantity)
                {
                    product.Quantity -= item.Quantity;
                }
            }
            await _productService.UpdateRangeAsync(paymentRequest.Products);

            var orderNumber = _orderService.GetNewOrderNumber();
            var totalProductQuantity = paymentRequest.Products.Sum(p => p.Quantity);

            var order = new Order
            {
                OrderNumber = orderNumber,
                UserId = paymentRequest.UserId,
                PaymentDate = DateTime.Now,
                PaymentMethod = "Kredi Kartı",
                PaymentNumber = payment.PaymentId,
                Status = "Hazırlanıyor",
                PaymentCurrency = paymentRequest.Currency,
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

            var user = await _userService.GetUserByIdAsync(paymentRequest.UserId);
            if (user is not null)
            {
                var baskets = await _basketRepository.Query().Where(p => p.UserId == paymentRequest.UserId).ToListAsync(cancellationToken);
                await _basketRepository.DeleteRangeAsync(baskets);
            }
            
        }
        else
        {
            throw new Exception(payment.ErrorMessage);
        }
    }
}