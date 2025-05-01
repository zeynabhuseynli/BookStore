using System;
using BookStore.Application.DTOs.AuthorDtos;
using FluentValidation;

namespace BookStore.Application.Validators.Authors;
public class UpdateAuthorDtoValidator : AbstractValidator<UpdateAuthorDto>
{
    public UpdateAuthorDtoValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Invalid Author Id.");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(50).WithMessage("First name must be less than 50 characters.");

        RuleFor(x => x.Description)
           .NotEmpty().WithMessage("Description is required.")
           .MaximumLength(500).WithMessage("Description must be less than 500 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(50).WithMessage("Last name must be less than 50 characters.");

        RuleFor(x => x.BirthDay)
           .LessThan(DateTime.UtcNow).WithMessage("Birthday must be in the past.");

        RuleFor(x => x.DeathTime)
            .GreaterThan(x => x.BirthDay)
            .When(x => x.DeathTime.HasValue)
            .WithMessage("Death date must be after birth date.");
    }
}

