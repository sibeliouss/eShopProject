using Application.Services.Repositories;
using MediatR;

namespace Application.Features.WishLists.Commands.Delete;

public class DeleteWishListCommand : IRequest
{
    public Guid Id { get; set; }
    
    public class DeleteWishListCommandHandler:IRequestHandler<DeleteWishListCommand>
    {
        private readonly IWishListRepository _wishListRepository;

        public DeleteWishListCommandHandler(IWishListRepository wishListRepository)
        {
            _wishListRepository = wishListRepository;
        }
        public async Task Handle(DeleteWishListCommand request, CancellationToken cancellationToken)
        {
            var wishList = await _wishListRepository.GetByIdAsync(request.Id);
            if (wishList is not null)
            {
                await _wishListRepository.DeleteAsync(wishList);
            }
        }
    }
}