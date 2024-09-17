using Application.Services.Repositories;
using AutoMapper;
using MediatR;
using MediatR.Pipeline;

namespace Application.Features.Categories.Commands.Update;

public class UpdateCategoryCommand : IRequest<UpdatedCategoryResponse>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string IconImgUrl { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }

    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, UpdatedCategoryResponse>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }
        public async Task<UpdatedCategoryResponse> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(request.Id);
            if (category is null)
            {
                throw new Exception("Güncellenecek kategori bulunmadı!");
            }

            /*category = _mapper.Map(request,category);*/
            category.Name = request.Name;
            category.IconImgUrl = request.IconImgUrl;
            category.IsActive = true;
            category.IsDeleted = false;

            await _categoryRepository.UpdateAsync(category);

            var response = _mapper.Map<UpdatedCategoryResponse>(category);
             return response;
        }
    }
}