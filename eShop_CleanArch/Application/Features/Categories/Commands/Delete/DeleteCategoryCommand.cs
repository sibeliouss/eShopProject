using Application.Services.Repositories;
using MediatR;

namespace Application.Features.Categories.Commands.Delete;

public class DeleteCategoryCommand : IRequest
{
    public Guid Id { get; set; }
    
    public class DeleteCategoryCommandHandler: IRequestHandler<DeleteCategoryCommand>
    {
        private readonly ICategoryRepository _categoryRepository;

        public DeleteCategoryCommandHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public async Task Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(request.Id);
            if (category is null)
            {
                throw new Exception("Böyle bir kategori mevcut değil.");
            }

            category.IsDeleted = true;

            await _categoryRepository.DeleteAsync(category);
        }
    }
}