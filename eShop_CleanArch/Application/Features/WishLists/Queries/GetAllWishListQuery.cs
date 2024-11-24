using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Features.Products.Dtos;
using Application.Features.WishLists.Queries.Responses;
using Application.Services.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.WishLists.Queries;

public class GetAllWishListQuery : IRequest<List<WishListResponse>>
{
   public Guid UserId { get; set; } 
   
   public class GetAllWishListQueryHandler : IRequestHandler<GetAllWishListQuery, List<WishListResponse>>
   {
      
      private readonly IWishListRepository _wishListRepository;

      public GetAllWishListQueryHandler(IWishListRepository wishListRepository)
      {
         _wishListRepository = wishListRepository;
      }
      public async Task<List<WishListResponse>> Handle(GetAllWishListQuery request, CancellationToken cancellationToken)
      {
         
         var currentDate = DateTime.Now; 
         var wishLists = await _wishListRepository.Query()
            .Where(p => p.UserId == request.UserId)
            .AsNoTracking()
            .Include(p => p.Product)
            .Select(s => new WishListResponse
            {
               CreateAt = s.Product.CreateAt,
               Id = s.Product.Id,
               IsActive = s.Product.IsActive,
               Img = s.Product.Img,
               IsDeleted = s.Product.IsDeleted,
               IsFeatured = s.Product.IsFeatured,
               ProductDetail = s.Product.ProductDetail != null
                  ? new ProductDetailDto
                  {
                     Id = s.Product.ProductDetail.Id,
                     ProductId = s.Product.Id,
                     Barcode = s.Product.ProductDetail.Barcode,
                     Description = s.Product.ProductDetail.Description,
                     Material = s.Product.ProductDetail.Material,
                     Fit = s.Product.ProductDetail.Fit,
                     Size = s.Product.ProductDetail.Size,
                     Color = s.Product.ProductDetail.Color
                  }
                  : null,
               ProductDiscount = s.Product.ProductDiscount != null &&
                                 s.Product.ProductDiscount.StartDate <= currentDate && 
                                 s.Product.ProductDiscount.EndDate >= currentDate   
                  ? new ProductDiscountDto()
                  {
                     Id = s.Product.ProductDiscount.Id,
                     ProductId = s.Product.Id,
                     DiscountedPrice = s.Product.ProductDiscount.DiscountedPrice,
                     DiscountPercentage = s.Product.ProductDiscount.DiscountPercentage,
                     StartDate = s.Product.ProductDiscount.StartDate,
                     EndDate = s.Product.ProductDiscount.EndDate
                  }
                  : null,
               Name = s.Product.Name,
               Brand = s.Product.Brand,
               ProductCategories = s.Product.ProductCategories.ToList(), // Null olamayacak şekilde
               Price = s.Product.Price,
               WishListId = s.Id,
            
            }).ToListAsync(cancellationToken);

         return wishLists;
      }
   }
}