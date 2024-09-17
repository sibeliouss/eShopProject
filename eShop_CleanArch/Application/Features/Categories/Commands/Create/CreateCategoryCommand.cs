using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Application.Features.Categories.Commands.Create;

public class CreateCategoryCommand : IRequest<CreatedCategoryResponse>
{

    public string Name { get; set; }
    public string IconImgUrl { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; } = false;

    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, CreatedCategoryResponse>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CreateCategoryCommandHandler(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }
        public async Task<CreatedCategoryResponse> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var categoryExists = await _categoryRepository.AnyAsync(c => c.Name == request.Name);
            if (categoryExists)
            {
                throw new Exception("Bu kategori zaten mevcut!");
            }

            var category = new Category
            {
                Name = request.Name,
                IconImgUrl = request.IconImgUrl,
                IsActive = true,
                IsDeleted = false
            };

            await _categoryRepository.AddAsync(category);

            // var response = _mapper.Map<CreatedCategoryResponse>(category);
            var response = new CreatedCategoryResponse()
            {
                Id = category.Id,
                Name = category.Name,
                IconImgUrl = category.IconImgUrl
            };
            return response;
        }
    }
}