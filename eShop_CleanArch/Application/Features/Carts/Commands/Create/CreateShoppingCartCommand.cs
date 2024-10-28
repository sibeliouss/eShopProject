using Application.Features.Baskets.Dtos;
using Application.Features.Carts.Commands.Create;
using Application.Services.Products;
using Application.Services.Repositories;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Application.Features.Baskets.Commands.Create;

public class CreateShoppingCartCommand : IRequest<CreatedShoppingCartResponse>
{
   public ShoppingCartDto ShoppingCartDto { get; set; }
   
   public class CreateShoppingCartCommandHandler : IRequestHandler<CreateShoppingCartCommand, CreatedShoppingCartResponse>
   {
       private readonly ICartRepository _cartRepository;
       private readonly IProductService _productService;
       private readonly IValidator<ShoppingCartDto> _validator;

       public CreateShoppingCartCommandHandler(ICartRepository cartRepository, IProductService productService, IValidator<ShoppingCartDto> validator)
       {
           _cartRepository = cartRepository;
           _productService = productService;
           _validator = validator;
       }
       
       public async Task<CreatedShoppingCartResponse> Handle(CreateShoppingCartCommand request, CancellationToken cancellationToken)
       {
           var shoppingCartDto = request.ShoppingCartDto;
           
           var validationResult = await _validator.ValidateAsync(shoppingCartDto, cancellationToken);
           if (!validationResult.IsValid)
           {
               throw new ValidationException(validationResult.Errors);
           }

           var product = await _productService.GetByIdAsync(shoppingCartDto.ProductId);
           if (product is null) throw new Exception("Ürün bulunamadı!");
           if (product.Quantity == 0) throw new Exception("Ürün stokta kalmadı!");
           
           
           var cart = await _cartRepository.Query()
               .Where(b => b.ProductId == request.ShoppingCartDto.ProductId &&
                           b.UserId == request.ShoppingCartDto.UserId).FirstOrDefaultAsync(cancellationToken);

           if (cart is not null)
           {
               if (product.Quantity<=cart.Quantity)
               {
                   throw new Exception("Ürün stokta kalmadı!");
               }

               cart.Quantity += 1;
           }
           else
           {
               cart = new Cart()
               {
                 ProductId = shoppingCartDto.ProductId,
                 Price = shoppingCartDto.Price,
                 Quantity = 1,
                 UserId = shoppingCartDto.UserId
               };
               await _cartRepository.AddAsync(cart);
           }

           return new CreatedShoppingCartResponse()
           {
               ProductId = cart.ProductId,
               UserId = cart.UserId,
               Price = cart.Price,
               Quantity = cart.Quantity
           };
       }
   }
}