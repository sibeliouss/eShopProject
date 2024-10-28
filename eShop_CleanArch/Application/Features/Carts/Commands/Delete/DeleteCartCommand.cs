using Application.Services.Repositories;
using MediatR;

namespace Application.Features.Carts.Commands.Delete;

public class DeleteCartCommand : IRequest
{
    public Guid Id { get; set; }
    
    public class DeleteCartCommandHandler : IRequestHandler<DeleteCartCommand>
    {
        private readonly ICartRepository _cartRepository;

        public DeleteCartCommandHandler(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }
        public async Task Handle(DeleteCartCommand request, CancellationToken cancellationToken)
        {
            var cart = await _cartRepository.GetByIdAsync(request.Id);
            if (cart is not null)
            {
                await _cartRepository.DeleteAsync(cart);
            }
        }
    }
}