using Microsoft.AspNetCore.Http;

namespace Core.Interfaces
{
    public interface ICloudinaryService
    {
        Task<string> UploadAsync(IFormFile file);
    }
}
