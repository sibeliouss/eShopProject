using Application.Services.Repositories;
using MediatR;

namespace Application.Features.BillingAddresses.Commands.Delete;

public class DeleteBillingAddressCommand : IRequest
{
    public Guid Id { get; set; }

    public class DeleteAddressCommandHandler : IRequestHandler<DeleteBillingAddressCommand>
    {
        private readonly IBillingAddressRepository _billingAddressRepository;

        public DeleteAddressCommandHandler(IBillingAddressRepository billingAddressRepository)
        {
            _billingAddressRepository = billingAddressRepository;
        }
        public async Task Handle(DeleteBillingAddressCommand request, CancellationToken cancellationToken)
        {
            var billingAddress = await _billingAddressRepository.GetByIdAsync(request.Id);
            if (billingAddress == null)
            {
                throw new Exception("Kayıt bulunamadı!");
            }

            await _billingAddressRepository.DeleteAsync(billingAddress);
        }
    } 
}