namespace Application.Features.Categories.Dtos;

public record GetListCategoryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string IconImgUrl { get; set; }
}