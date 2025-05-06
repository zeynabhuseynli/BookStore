using BookStore.Application.DTOs.UserDtos;
using FluentValidation;

namespace BookStore.Application.Validators.Users;
public class LoginDtoValidator : AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
    }
}

