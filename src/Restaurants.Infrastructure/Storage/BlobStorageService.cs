using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Microsoft.Extensions.Options;
using Restaurants.Domain.Interfaces;
using Restaurants.Infrastructure.Configurations;

namespace Restaurants.Infrastructure.Storage;
internal class BlobStorageService(IOptions<BlobStorageOptions> options) : IBlobStorageService
{
    private readonly BlobStorageOptions blobStorageOptions = options.Value;
    public async Task<string> UploadAsync(string FileName, Stream Data)
    {
        var client = new BlobServiceClient(blobStorageOptions.ConnectionString.Trim());

        var container = client.GetBlobContainerClient(blobStorageOptions.ContainerName);
        container.CreateIfNotExists();

        var blobClient = container.GetBlobClient(FileName);

        await blobClient.UploadAsync(Data, true);

        return blobClient.Uri.ToString();
    }

    public string? GetUrl(string? blobUrl)
    {
        if (string.IsNullOrEmpty(blobUrl))
        {
            return null;
        }

        var sasBuilder = new BlobSasBuilder()
        {
            BlobContainerName = blobStorageOptions.ContainerName,
            Resource = "b",
            StartsOn = DateTimeOffset.UtcNow,
            ExpiresOn = DateTimeOffset.UtcNow.AddHours(1),
            BlobName = GetBlobNameFromUrl(blobUrl)
        };

        sasBuilder.SetPermissions(BlobSasPermissions.Read);

        var credintials = new StorageSharedKeyCredential(blobStorageOptions.AccountName, blobStorageOptions.AccountKey);
        var sasToken = sasBuilder.ToSasQueryParameters(credintials).ToString();
        return $"{blobUrl}?{sasToken}";
    }
    public async Task DeleteAsync(string blobUrl)
    {
        var client = new BlobServiceClient(blobStorageOptions.ConnectionString);
        var container = client.GetBlobContainerClient(blobStorageOptions.ContainerName);
        var fileName = GetBlobNameFromUrl(blobUrl);
        var blobClient = container.GetBlobClient(fileName);
        await blobClient.DeleteIfExistsAsync();
    }

    private string GetBlobNameFromUrl(string blobUrl)
    {
        var uri = new Uri(blobUrl);
        return uri.Segments.Last();
    }
}