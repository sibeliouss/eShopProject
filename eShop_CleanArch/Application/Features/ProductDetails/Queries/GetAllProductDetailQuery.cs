using Application.Services.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.ProductDetails.Queries;

public class GetAllProductDetailQuery : IRequest<List<GetAllProductDetailResponse>>
{

    public class GetAllProductDetailQueryHandler : IRequestHandler<GetAllProductDetailQuery, List<GetAllProductDetailResponse>>
    {
        private readonly IProductDetailRepository _detailRepository;

        public GetAllProductDetailQueryHandler(IProductDetailRepository detailRepository)
        {
            _detailRepository = detailRepository;
        }
        public async Task<List<GetAllProductDetailResponse>> Handle(GetAllProductDetailQuery request, CancellationToken cancellationToken)
        {
            var productDetail = await _detailRepository.Query().AsNoTracking().Select(pd => new GetAllProductDetailResponse
                {
                    Id = pd.Id,
                    Barcode = pd.Barcode,
                    Description = pd.Description,
                    Color = pd.Color,
                    Fit = pd.Fit,
                    Size = pd.Size,
                    Stock = pd.Stock,
                    Material = pd.Material
                })
                .ToListAsync(cancellationToken);

            return productDetail;
        }
    }
}