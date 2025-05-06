using BookStore.Application.DTOs.UserDtos;
using FluentValidation;

namespace BookStore.Application.Validators.Users;
public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
{
    public UpdateUserDtoValidator()
    {
        RuleFor(x => x.UserId).GreaterThan(0);
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.BirthDate).LessThan(DateTime.Now);
    }
}

