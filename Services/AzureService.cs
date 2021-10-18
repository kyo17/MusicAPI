using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Interfaces;
using Microsoft.Extensions.Configuration;

namespace Services
{
    public class AzureService : IAzure
    {
        private readonly string azure;

        public AzureService(IConfiguration config)
        {
            azure = config.GetConnectionString("Azure");
        }

        public async Task<string> edit(byte[] content, string ext, string folder, string path)
        {
            await remove(path, folder);
            return await save(content, ext, folder);
        }

        public async Task remove(string path, string folder)
        {
            if (string.IsNullOrEmpty(path))
            {
                return;
            }
            var client = new BlobContainerClient(azure, folder);
            await client.CreateIfNotExistsAsync();
            var fileName = Path.GetFileName(path);
            var blob = client.GetBlobClient(fileName);
            await blob.DeleteIfExistsAsync();
        }

        public async Task<string> save(byte[] content, string ext, string folder)
        {
            var client = new BlobContainerClient(azure, folder);
            await client.CreateIfNotExistsAsync();
            client.SetAccessPolicy(PublicAccessType.Blob);
            var file = $"{Guid.NewGuid().ToString().Substring(14)}";
            var blob = client.GetBlobClient(file);
            using(var ms = new MemoryStream(content))
            {
                await blob.UploadAsync(ms);
            }
            return blob.Uri.ToString();
        }
    }
}
