using Application.Services.Repositories;
using MediatR;

namespace Application.Features.ProductDiscounts.Commands.Delete;

public class DeleteProductDiscountCommand : IRequest
{
    public Guid Id { get; set; }
    
    public class DeleteProductDiscountCommandHandler : IRequestHandler<DeleteProductDiscountCommand>
    {
        private readonly IProductDiscountRepository _productDiscountRepository;

        public DeleteProductDiscountCommandHandler(IProductDiscountRepository productDiscountRepository)
        {
            _productDiscountRepository = productDiscountRepository;
        }
        public async Task Handle(DeleteProductDiscountCommand request, CancellationToken cancellationToken)
        {
            var productDiscount = await _productDiscountRepository.GetByIdAsync(request.Id);

            if (productDiscount != null) 
                await _productDiscountRepository.DeleteAsync(productDiscount);
        }
    }
}