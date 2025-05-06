using BookStore.Application.DTOs.UserDtos;
using BookStore.Infrastructure.BaseMessages;
using FluentValidation;

namespace BookStore.Application.Validators.Users;
public class RegisterDtoValidator : AbstractValidator<RegisterDto>
{
    public RegisterDtoValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
        RuleFor(x => x.BirthDay).LessThan(DateTime.Now).WithMessage(UIMessage.BIRTHDAY_MESSAGE);
    }
}

