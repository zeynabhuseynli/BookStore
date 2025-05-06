using BookStore.Application.DTOs.UserDtos;
using FluentValidation;

namespace BookStore.Application.Validators.Users;
public class UpdateRoleDtoValidator : AbstractValidator<UpdateRoleDto>
{
    public UpdateRoleDtoValidator()
    {
        RuleFor(x => x.UserId).GreaterThan(0);
        RuleFor(x => x.Role).IsInEnum();
    }
}

