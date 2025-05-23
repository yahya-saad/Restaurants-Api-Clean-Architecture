namespace Restaurants.Domain.Interfaces;
public interface IBlobStorageService
{
    Task<string> UploadAsync(string FileName, Stream Data);
    string? GetUrl(string blobUrl);
    Task DeleteAsync(string blobUrl);
}
