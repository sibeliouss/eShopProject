namespace Application.Features.Categories.Commands.Create;

public class CreatedCategoryResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string IconImgUrl { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; } = false;

}