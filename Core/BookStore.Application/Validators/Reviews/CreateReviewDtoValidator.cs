using BookStore.Application.DTOs.ReviewDtos;
using FluentValidation;

namespace BookStore.Application.Validators.Reviews;
public class CreateReviewDtoValidator : AbstractValidator<CreateReviewDto>
{
    public CreateReviewDtoValidator()
    {
        RuleFor(x => x.BookId).GreaterThan(0);
        RuleFor(x => x.Message).NotEmpty().MaximumLength(1000);

        RuleFor(x => x.Rating)
            .IsInEnum()
            .When(x => x.Rating.HasValue);

        RuleFor(x => x.ParentReviewId)
            .GreaterThan(0)
            .When(x => x.ParentReviewId.HasValue);
    }
}

