using Microsoft.AspNetCore.JsonPatch;

namespace Restaurants.Application.Restaurants.Commands.PatchRestaurant;
public class PatchRestaurantCommandValidator : AbstractValidator<PatchRestaurantCommand>
{
    public PatchRestaurantCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Id must be greater than 0.");

        RuleFor(x => x.PatchDocument)
            .NotNull()
            .WithMessage("Patch document cannot be null.")
            .Must(PatchOperationsAreValid)
            .WithMessage("Only 'replace' operations are allowed in the patch document.");




    }

    private bool PatchOperationsAreValid(JsonPatchDocument<PatchRestaurantDto> patchDocument)
    {
        return patchDocument.Operations.All(op =>
            op.op.Equals("replace", StringComparison.OrdinalIgnoreCase));
    }

}
