using Application.Services.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.ProductDetails.Queries;

public class GetAllProductDetailQuery : IRequest<List<GetAllProductDetailResponse>>
{

    public class GetAllProductDetailQueryHandler : IRequestHandler<GetAllProductDetailQuery, List<GetAllProductDetailResponse>>
    {
        private readonly IProductDetailRepository _detailRepository;
        private readonly IMapper _mapper;

        public GetAllProductDetailQueryHandler(IProductDetailRepository detailRepository, IMapper mapper)
        {
            _detailRepository = detailRepository;
            _mapper = mapper;
        }

        public async Task<List<GetAllProductDetailResponse>> Handle(GetAllProductDetailQuery request, CancellationToken cancellationToken)
        {
            var productDetails = await _detailRepository.Query().AsNoTracking().ToListAsync(cancellationToken);
            var response = _mapper.Map<List<GetAllProductDetailResponse>>(productDetails);
            return response;
        }
    }
}