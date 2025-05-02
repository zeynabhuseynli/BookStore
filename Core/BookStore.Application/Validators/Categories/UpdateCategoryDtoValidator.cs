using BookStore.Application.DTOs.Categories;
using FluentValidation;

namespace BookStore.Application.Validators.Categories;
public class UpdateCategoryDtoValidator : AbstractValidator<UpdateCategoryDto>
{
    public UpdateCategoryDtoValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage("Invalid ID.");
        Include(new CreateCategoryDtoValidator());
    }
}

