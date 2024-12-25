using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using StarterApi.Application.Common.Interfaces;

namespace StarterApi.Infrastructure.Services
{
    public class AzureBlobStorageService : IBlobStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _storageBaseUrl;

        public AzureBlobStorageService(IConfiguration configuration)
        {
            var connectionString = configuration["Azure:Storage:ConnectionString"]
                ?? throw new ArgumentNullException("Azure:Storage:ConnectionString", "Blob storage connection string is not configured");
            
            _blobServiceClient = new BlobServiceClient(connectionString);
            _storageBaseUrl = configuration["Azure:Storage:BaseUrl"]
                ?? throw new ArgumentNullException("Azure:Storage:BaseUrl", "Blob storage base URL is not configured");
        }

        public async Task<(string blobUrl, string blobPath)> UploadAsync(string containerName, string fileName, Stream content, string contentType)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            await containerClient.CreateIfNotExistsAsync();

            // Generate a unique blob name using a timestamp and original filename
            var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            var blobPath = $"{timestamp}_{fileName}";
            var blobClient = containerClient.GetBlobClient(blobPath);

            var blobHttpHeaders = new BlobHttpHeaders { ContentType = contentType };
            await blobClient.UploadAsync(content, new BlobUploadOptions { HttpHeaders = blobHttpHeaders });

            return (blobClient.Uri.ToString(), blobPath);
        }

        public async Task<Stream> DownloadAsync(string containerName, string blobPath)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(blobPath);

            var response = await blobClient.DownloadAsync();
            return response.Value.Content;
        }

        public async Task DeleteAsync(string containerName, string blobPath)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(blobPath);
            await blobClient.DeleteIfExistsAsync();
        }

        public async Task<bool> ExistsAsync(string containerName, string blobPath)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(blobPath);
            return await blobClient.ExistsAsync();
        }

        public string GetBlobUrl(string containerName, string blobPath)
        {
            return $"{_storageBaseUrl.TrimEnd('/')}/{containerName}/{blobPath}";
        }
    }
}
