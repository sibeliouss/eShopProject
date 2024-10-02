using Application.Features.ProductDetails.Constants;
using Application.Features.ProductDetails.Dtos;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.ProductDetails.Commands.Update;

public class UpdateProductDetailCommand :IRequest<UpdatedProductDetailResponse>
{
    public UpdateProductDetailDto UpdateProductDetailDto { get; set; }

    public class UpdateProductDetailCommandHandler: IRequestHandler<UpdateProductDetailCommand, UpdatedProductDetailResponse>
    {
        private readonly IProductDetailRepository _productDetailRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateProductDetailDto> _validator;

        public UpdateProductDetailCommandHandler(IProductDetailRepository productDetailRepository, IMapper mapper, IValidator<UpdateProductDetailDto> validator)
        {
            _productDetailRepository = productDetailRepository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<UpdatedProductDetailResponse> Handle(UpdateProductDetailCommand request, CancellationToken cancellationToken)
        {
            var productDetailDto = request.UpdateProductDetailDto;
            
            var validationResult = await _validator.ValidateAsync(productDetailDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            
            var productDetail = await _productDetailRepository.GetByIdAsync(productDetailDto.Id);
            if (productDetail is null)
            {
                throw new Exception(ProductDetailMessages.ProductDetailNotFound);
            }

            _mapper.Map(productDetailDto, productDetail);

            await _productDetailRepository.UpdateAsync(productDetail);

            return _mapper.Map<UpdatedProductDetailResponse>(productDetail);
            
        }
    }
}