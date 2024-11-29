using Application.Services.Products;
using Application.Services.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Carts.Queries;

public class ChangeProductQuantityInCart : IRequest
{
  public  Guid ProductId { get; set; }
  public int Quantity { get; set; }
  
  public ChangeProductQuantityInCart(Guid productId, int quantity)
  {
    ProductId = productId;
    Quantity = quantity;
  }
  
  public class ChangeProductQuantityInCartHandler : IRequestHandler<ChangeProductQuantityInCart>
  {
    private readonly ICartRepository _cartRepository;
    private readonly IProductService _productService;

    public ChangeProductQuantityInCartHandler(ICartRepository cartRepository, IProductService productService)
    {
      _cartRepository = cartRepository;
      _productService = productService;

    }

    public async Task Handle(ChangeProductQuantityInCart request, CancellationToken cancellationToken)
    {
      
      var cart = await _cartRepository.Query().Where(p => p.ProductId == request.ProductId).FirstOrDefaultAsync(cancellationToken);

      if (cart == null)
      {
        throw new Exception("Ürün bulunamadı");
      }

      
      if (request.Quantity == 0)
      {
        await _cartRepository.DeleteAsync(cart);  
      }
      else
      {
       
        var product = await _productService.FindAsync(request.ProductId);

        if (product == null)
        {
          throw new Exception("Ürün bulunamadı");
        }

        if (product.Quantity < request.Quantity)
        {
          throw new Exception("Ürün stoğu yeterli değil");
        }
        else
        {
         
          cart.Quantity = request.Quantity;
          await _cartRepository.UpdateAsync(cart); 
        }
      }

    }
  }
}