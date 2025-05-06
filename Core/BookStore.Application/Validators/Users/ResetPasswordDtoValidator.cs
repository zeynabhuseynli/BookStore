using BookStore.Application.DTOs.UserDtos;
using FluentValidation;

namespace BookStore.Application.Validators.Users;
public class ResetPasswordDtoValidator : AbstractValidator<ResetPasswordDto>
{
    public ResetPasswordDtoValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.OtpCode).GreaterThan(0);
        RuleFor(x => x.NewPassword).NotEmpty().MinimumLength(6);
    }
}

