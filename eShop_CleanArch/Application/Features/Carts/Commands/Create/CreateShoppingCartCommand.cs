using Application.Features.Carts.Dtos;
using Application.Services.Products;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Carts.Commands.Create;

public class CreateShoppingCartCommand : IRequest<CreatedShoppingCartResponse>
{
   public ShoppingCartDto ShoppingCartDto { get; set; }
   
   public class CreateShoppingCartCommandHandler : IRequestHandler<CreateShoppingCartCommand, CreatedShoppingCartResponse>
   {
       private readonly ICartRepository _cartRepository;
       private readonly IProductService _productService;
       private readonly IValidator<ShoppingCartDto> _validator;
       private readonly IMapper _mapper;

       public CreateShoppingCartCommandHandler(ICartRepository cartRepository, IProductService productService, IValidator<ShoppingCartDto> validator, IMapper mapper)
       {
           _cartRepository = cartRepository;
           _productService = productService;
           _validator = validator;
           _mapper = mapper;
       }
       
       public async Task<CreatedShoppingCartResponse> Handle(CreateShoppingCartCommand request, CancellationToken cancellationToken)
       {
           var shoppingCartDto = request.ShoppingCartDto;
           
           var validationResult = await _validator.ValidateAsync(shoppingCartDto, cancellationToken);
           if (!validationResult.IsValid)
           {
               throw new ValidationException(validationResult.Errors);
           }

           var product = await _productService.FindAsync(shoppingCartDto.ProductId);
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
               cart = _mapper.Map<Cart>(shoppingCartDto);
               await _cartRepository.AddAsync(cart);

           }

           var response = _mapper.Map<CreatedShoppingCartResponse>(cart);

           return response;
       }
   }
}