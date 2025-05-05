using System;
using BookStore.Application.DTOs.AuthorDtos;
using BookStore.Infrastructure.BaseMessages;
using FluentValidation;

namespace BookStore.Application.Validators.Authors;
public class UpdateAuthorDtoValidator : AbstractValidator<UpdateAuthorDto>
{
    public UpdateAuthorDtoValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Invalid Author Id.");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage(UIMessage.GetRequiredMessage("FirstName"))
            .MaximumLength(50).WithMessage(UIMessage.GetMaxLengthMessage(50,"FirtName"));

        RuleFor(x => x.Description)
           .NotEmpty().WithMessage(UIMessage.GetRequiredMessage("Description"))
           .MaximumLength(500).WithMessage(UIMessage.GetMaxLengthMessage(500, "Description"));

        RuleFor(x => x.LastName)
             .NotEmpty().WithMessage(UIMessage.GetRequiredMessage("LastName"))
            .MaximumLength(50).WithMessage(UIMessage.GetMaxLengthMessage(50, "LastName"));

        RuleFor(x => x.BirthDay)
           .LessThan(DateTime.UtcNow).WithMessage("Birthday must be in the past.");

        RuleFor(x => x.DeathTime)
            .GreaterThan(x => x.BirthDay)
            .When(x => x.DeathTime.HasValue)
            .WithMessage("Death date must be after birth date.");
    }
}

