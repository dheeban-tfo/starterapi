using System.IO;
using System.Threading.Tasks;

namespace StarterApi.Application.Common.Interfaces
{
    public interface IBlobStorageService
    {
        Task<(string blobUrl, string blobPath)> UploadAsync(string containerName, string fileName, Stream content, string contentType);
        Task<Stream> DownloadAsync(string containerName, string blobPath);
        Task DeleteAsync(string containerName, string blobPath);
        Task<bool> ExistsAsync(string containerName, string blobPath);
        string GetBlobUrl(string containerName, string blobPath);
    }
}
