using Microsoft.AspNetCore.Http;

namespace FastTechFoods.Application.Interfaces;

public interface IBlobStorageService
{
    Task<string> UploadImageAsync(IFormFile file, string containerName, CancellationToken cancellationToken = default);
    Task<bool> DeleteImageAsync(string imageUrl, string containerName, CancellationToken cancellationToken = default);
}