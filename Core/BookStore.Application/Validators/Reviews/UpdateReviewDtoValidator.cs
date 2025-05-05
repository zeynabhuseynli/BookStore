using BookStore.Application.DTOs.ReviewDtos;
using FluentValidation;

namespace BookStore.Application.Validators.Reviews;
public class UpdateReviewDtoValidator : AbstractValidator<UpdateReviewDto>
{
    public UpdateReviewDtoValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Message).NotEmpty().MaximumLength(1000);

        RuleFor(x => x.Rating)
            .IsInEnum()
            .When(x => x.Rating.HasValue);
    }
}

