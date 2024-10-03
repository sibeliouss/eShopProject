using Application.Services.Repositories;
using MediatR;

namespace Application.Features.Baskets.Commands.Delete;

public class DeleteBasketCommand : IRequest
{
    public Guid Id { get; set; }
    
    public class DeleteBasketCommandHandler : IRequestHandler<DeleteBasketCommand>
    {
        private readonly IBasketRepository _basketRepository;

        public DeleteBasketCommandHandler(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
        }
        public async Task Handle(DeleteBasketCommand request, CancellationToken cancellationToken)
        {
            var basket = await _basketRepository.GetByIdAsync(request.Id);
            if (basket is not null)
            {
                await _basketRepository.DeleteAsync(basket);
            }
        }
    }
}