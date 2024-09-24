/*using Application.Features.ProductDetails.Dtos;
using Application.Services.Repositories;
using MediatR;

namespace Application.Features.ProductDetails.Commands.Update;

public class UpdateProductDetailCommand :IRequest<UpdatedProductDetailResponse>
{
    public UpdateProductDetailDto UpdateProductDetailDto { get; set; }
    
    
    public class UpdateProductDetailCommandHandler: IRequestHandler<UpdateProductDetailCommand, UpdatedProductDetailResponse>
    {
        private readonly IProductDetailRepository _productDetailRepository;

        public UpdateProductDetailCommandHandler(IProductDetailRepository productDetailRepository)
        {
            _productDetailRepository = productDetailRepository;
        }
        
        public async Task<UpdatedProductDetailResponse> Handle(UpdateProductDetailCommand request, CancellationToken cancellationToken)
        {
            var productDetailDto = request.UpdateProductDetailDto;
            var productDetail = _productDetailRepository.AnyAsync(pd => pd.Id == productDetailDto.Id);
            
        }
    }
}*/