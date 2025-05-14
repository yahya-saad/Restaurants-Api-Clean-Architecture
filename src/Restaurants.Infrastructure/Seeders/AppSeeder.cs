
using Microsoft.Extensions.DependencyInjection;

namespace Restaurants.Infrastructure.Seeders;
public class AppSeeder
{
    private readonly IServiceProvider _serviceProvider;
    public AppSeeder(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        using var scope = _serviceProvider.CreateScope();
        var seeders = scope.ServiceProvider.GetServices<ISeeder>(); // Get all ISeeder implementations

        foreach (var seeder in seeders)
        {
            await seeder.SeedAsync(cancellationToken);
        }
    }
}
