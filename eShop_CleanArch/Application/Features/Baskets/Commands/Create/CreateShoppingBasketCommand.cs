using Application.Features.Baskets.Dtos;
using Application.Services.Products;
using Application.Services.Repositories;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Baskets.Commands.Create;

public class CreateShoppingBasketCommand : IRequest<CreatedShoppingBasketResponse>
{
   public ShoppingBasketDto ShoppingBasketDto { get; set; }
   
   public class CreateShoppingBasketCommandHandler : IRequestHandler<CreateShoppingBasketCommand, CreatedShoppingBasketResponse>
   {
       private readonly IBasketRepository _basketRepository;
       private readonly IProductService _productService;
       private readonly IValidator<ShoppingBasketDto> _validator;

       public CreateShoppingBasketCommandHandler(IBasketRepository basketRepository, IProductService productService, IValidator<ShoppingBasketDto> validator)
       {
           _basketRepository = basketRepository;
           _productService = productService;
           _validator = validator;
       }
       
       public async Task<CreatedShoppingBasketResponse> Handle(CreateShoppingBasketCommand request, CancellationToken cancellationToken)
       {
           var shoppingBasketDto = request.ShoppingBasketDto;
           
           var validationResult = await _validator.ValidateAsync(shoppingBasketDto, cancellationToken);
           if (!validationResult.IsValid)
           {
               throw new ValidationException(validationResult.Errors);
           }

           var product = await _productService.GetByIdAsync(shoppingBasketDto.ProductId);
           if (product is null) throw new Exception("Ürün bulunamadı!");
           if (product.Quantity == 0) throw new Exception("Ürün stokta kalmadı!");
           
           
           var basket = await _basketRepository.Query()
               .Where(b => b.ProductId == request.ShoppingBasketDto.ProductId &&
                           b.CustomerId == request.ShoppingBasketDto.CustomerId).FirstOrDefaultAsync(cancellationToken);

           if (basket is not null)
           {
               if (product.Quantity<=basket.Quantity)
               {
                   throw new Exception("Ürün stokta kalmadı!");
               }

               basket.Quantity += 1;
           }
           else
           {
               basket = new Basket()
               {
                 ProductId = shoppingBasketDto.ProductId,
                 Price = shoppingBasketDto.Price,
                 Quantity = 1,
                 CustomerId = shoppingBasketDto.CustomerId
               };
               await _basketRepository.AddAsync(basket);
           }

           return new CreatedShoppingBasketResponse()
           {
               ProductId = basket.ProductId,
               CustomerId = basket.CustomerId,
               Price = basket.Price,
               Quantity = basket.Quantity
           };
       }
   }
}