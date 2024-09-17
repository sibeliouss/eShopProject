using Application.Features.Categories.Dtos;
using Application.Services.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Categories.Queries;

public class GetAllCategoriesQuery : IRequest<IEnumerable<GetListCategoryDto>>
{
}

public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, IEnumerable<GetListCategoryDto>>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public GetAllCategoriesQueryHandler(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GetListCategoryDto>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.Query().Where(p => p.IsActive == true && p.IsDeleted == false)
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken: cancellationToken);
        return _mapper.Map<IEnumerable<GetListCategoryDto>>(categories);
    }
}
