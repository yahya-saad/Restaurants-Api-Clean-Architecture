using Restaurants.Domain.Constants;

namespace Restaurants.Application.Users.Commands.UnassignUserRole;
public class UnassignUserRoleCommandValidator : AbstractValidator<UnassignUserRoleCommand>
{
    public UnassignUserRoleCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("User Email is required.")
            .EmailAddress().WithMessage("Invalid email address format.");

        RuleFor(x => x.Role)
            .NotEmpty().WithMessage("Role is required.")
            .Must(role => UserRoles.AllRoles.Contains(role))
            .WithMessage($"Role must be one of the following: {string.Join(", ", UserRoles.AllRoles)}.");
    }
}

