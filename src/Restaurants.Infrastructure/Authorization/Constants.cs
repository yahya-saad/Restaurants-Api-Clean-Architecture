namespace Restaurants.Infrastructure.Authorization;
public static class PolicyNames
{
    public const string HasNationality = "HasNationality";
    public const string AtLeast18 = "AtLeast18";
}
internal static class AppClaimTypes
{
    public const string Nationality = "Nationality";
    public const string DateOfBirth = "DateOfBirth";
}
