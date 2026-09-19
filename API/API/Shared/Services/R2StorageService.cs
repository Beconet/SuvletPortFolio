using Amazon.S3;
using Amazon.S3.Model;
using API.Configuration;
using Microsoft.Extensions.Options;

namespace API.Shared.Services
{
    public class R2StorageService
    {
        private readonly IAmazonS3 _s3Client;
        private readonly CloudflareR2Options _options;

        public R2StorageService(IAmazonS3 s3Client, IOptions<CloudflareR2Options> options)
        {
            _s3Client = s3Client;
            _options = options.Value;
        }

        public async Task<string> UploadFileAsync(IFormFile file, string folder, CancellationToken ct = default)
        {
            var fileName = $"{folder}/{Guid.NewGuid()}_{file.FileName}";

            using var stream = file.OpenReadStream();
            var request = new PutObjectRequest
            {
                BucketName = _options.BucketName,
                Key = fileName,
                InputStream = stream,
                ContentType = file.ContentType,
                DisablePayloadSigning = true
            };

            await _s3Client.PutObjectAsync(request, ct);
            return $"{_options.PublicAccessUrl.TrimEnd('/')}/{fileName}";
        }

        public async Task<bool> DeleteFileAsync(string fileUrl, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(fileUrl)) return false;

            var cleanUrl = fileUrl.Trim().Trim('"').Trim('\'');

            if (!Uri.TryCreate(cleanUrl, UriKind.Absolute, out var uri))
            {
                return false;
            }

            var rawPath = uri.AbsolutePath.TrimStart('/');


            var deleteRequest = new DeleteObjectRequest
            {
                BucketName = _options.BucketName,
                Key = rawPath
            };

            var response = await _s3Client.DeleteObjectAsync(deleteRequest, ct);

            if (response.HttpStatusCode == System.Net.HttpStatusCode.NoContent ||
                response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                var unescapedKey = Uri.UnescapeDataString(rawPath);
                if (unescapedKey != rawPath)
                {
                    await _s3Client.DeleteObjectAsync(new DeleteObjectRequest
                    {
                        BucketName = _options.BucketName,
                        Key = unescapedKey
                    }, ct);
                }
                return true;
            }

            return false;
        }
    }
}