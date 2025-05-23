using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Restaurants.Infrastructure.Configurations;
public class BlobStorageSetup : IConfigureOptions<BlobStorageOptions>
{
    private const string SectionName = "BlobStorage";
    private readonly IConfiguration _configuration;
    public BlobStorageSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(BlobStorageOptions options)
    {
        _configuration
            .GetSection(SectionName)
            .Bind(options);
    }
}
