namespace Restaurants.Infrastructure.Authorization;
public static class PolicyNames
{
    public const string HasNationality = "HasNationality";
    public const string AtLeast18 = "AtLeast18";
    public const string CanDelete = "CanDelete";
    public const string OwnsAtLeastTwoRestaurants = "OwnsAtLeastTwoRestaurants";

}
internal static class AppClaimTypes
{
    public const string Nationality = "Nationality";
    public const string DateOfBirth = "DateOfBirth";
}
