using Application.Services.Repositories;
using MediatR;

namespace Application.Features.Products.Commands.Delete;

public class DeleteProductCommand : IRequest
{
    public Guid Id { get; set; }

    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
    {
        private readonly IProductRepository _productRepository;

        public DeleteProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var findProductId = await _productRepository.FindAsync(request.Id);
            if (findProductId is null)
            {
                throw new Exception("Ürün kaydı bulunamadı");
                
            }

            await _productRepository.DeleteAsync(findProductId);

        }
    }
}