using Application.Features.Addresses.Rules;
using Application.Services.Repositories;
using MediatR;

namespace Application.Features.Addresses.Commands.Delete;

public class DeleteAddressCommand : IRequest
{
    public Guid Id { get; set; }

    public class DeleteAddressCommandHandler : IRequestHandler<DeleteAddressCommand>
    {
        private readonly IAddressRepository _addressRepository;

        public DeleteAddressCommandHandler(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        public async Task Handle(DeleteAddressCommand request, CancellationToken cancellationToken)
        {
            var address = await _addressRepository.GetByIdAsync(request.Id);
           
            if (address == null)
            {
                throw new Exception("Kayıt bulunamadı!");
            }

            await _addressRepository.DeleteAsync(address);
        }
    } 
}