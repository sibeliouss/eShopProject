using Application.Services.Repositories;
using MediatR;

namespace Application.Features.ProductDetails.Commands.Delete;

public class DeleteProductDetailCommand : IRequest
{
    public Guid Id { get; set; }
    
    public class DeleteProductDetailCommandHandler : IRequestHandler<DeleteProductDetailCommand>
    {
        private readonly IProductDetailRepository _productDetailRepository;

        public DeleteProductDetailCommandHandler(IProductDetailRepository productDetailRepository)
        {
            _productDetailRepository = productDetailRepository;
        }
        public async Task Handle(DeleteProductDetailCommand request, CancellationToken cancellationToken)
        {
            var findProductDetailId = await _productDetailRepository.GetByIdAsync(request.Id);
            if (findProductDetailId is null)
            {
                throw new Exception("Ürün kaydı bulunamadı");
                
            }

            await _productDetailRepository.DeleteAsync(findProductDetailId);
        }
    }
}