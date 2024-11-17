using Application.Services.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Orders.Commands.Delete;

public class DeleteOrderCommand: IRequest
{
    public Guid Id { get; set; }
    
    public class DeleteCartCommandHandler : IRequestHandler<DeleteOrderCommand>
    {
        private readonly IOrderRepository _orderRepository;

        public DeleteCartCommandHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public async Task Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.Query().Where(o=>o.Id==request.Id).FirstOrDefaultAsync(cancellationToken);
            if (order is not null)
            {
                await _orderRepository.DeleteAsync(order);
            }
        }
    }
}
