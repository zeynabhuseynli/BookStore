using BookStore.Application.DTOs.UserDtos;
using FluentValidation;

namespace BookStore.Application.Validators.Users;
public class ForgotPasswordDtoValidator : AbstractValidator<ForgotPasswordDto>
{
    public ForgotPasswordDtoValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
    }
}

