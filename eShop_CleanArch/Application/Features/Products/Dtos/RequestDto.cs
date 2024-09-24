namespace Application.Features.Products.Dtos;

public record RequestDto
(
    Guid CategoryId,
    int PageNumber,
    int PageSize,
    string Search,
    string OrderBy
);