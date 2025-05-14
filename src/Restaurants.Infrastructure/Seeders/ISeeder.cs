namespace Restaurants.Infrastructure.Seeders;
internal interface ISeeder
{
    Task SeedAsync(CancellationToken cancellationToken = default);
}
