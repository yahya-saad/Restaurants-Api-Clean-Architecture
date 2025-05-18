namespace Restaurants.Application.Users.Commands.UpdateUser;
internal class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.Dto.FirstName)
            .NotEmpty()
            .WithMessage("First name is required.")
            .MaximumLength(50)
            .WithMessage("First name must not exceed 50 characters.");

        RuleFor(x => x.Dto.LastName)
            .NotEmpty()
            .WithMessage("Last name is required.")
            .MaximumLength(50)
            .WithMessage("Last name must not exceed 50 characters.");

        RuleFor(x => x.Dto.Nationality)
            .NotEmpty()
            .WithMessage("Nationality is required.")
            .MaximumLength(50)
            .WithMessage("Nationality must not exceed 50 characters.");

        RuleFor(x => x.Dto.DateOfBirth)
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(new DateTime(1900, 1, 1)))
            .WithMessage("Date of birth must be on or after 01/01/1900.")
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("Date of birth cannot be in the future.");

    }
}
