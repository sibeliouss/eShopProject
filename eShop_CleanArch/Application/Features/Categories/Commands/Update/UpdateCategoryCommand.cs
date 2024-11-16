using Application.Features.Addresses.Commands.Update;
using Application.Features.Categories.Constants;
using Application.Features.Categories.Rules;
using Application.Features.Products.Rules;
using Application.Services.Repositories;
using AutoMapper;
using FluentValidation;
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
        private readonly IValidator<UpdateCategoryCommand> _validator;
        private readonly CategoryBusinessRules _categoryBusinessRules;

        public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository, IMapper mapper, IValidator<UpdateCategoryCommand> validator, CategoryBusinessRules categoryBusinessRules)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _validator = validator;
            _categoryBusinessRules = categoryBusinessRules;
        }
        public async Task<UpdatedCategoryResponse> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            
            var category = await _categoryRepository.FindAsync(request.Id);
            if (category == null)
            {
                throw new Exception(CategoryMessages.CategoryNotFound);
            }

            category = _mapper.Map(request,category);
            
            await _categoryRepository.UpdateAsync(category);

            var response = _mapper.Map<UpdatedCategoryResponse>(category);
             return response;
        }
    }
}