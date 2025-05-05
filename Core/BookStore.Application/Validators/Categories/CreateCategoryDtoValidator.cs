using BookStore.Application.DTOs.Categories;
using BookStore.Infrastructure.BaseMessages;
using FluentValidation;

namespace BookStore.Application.Validators.Categories;

public class CreateCategoryDtoValidator : AbstractValidator<CreateCategoryDto>
{
    public CreateCategoryDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(UIMessage.GetRequiredMessage("FirstName"))
            .MaximumLength(100).WithMessage(UIMessage.GetMaxLengthMessage(100, "FirtName"));
    }
}

