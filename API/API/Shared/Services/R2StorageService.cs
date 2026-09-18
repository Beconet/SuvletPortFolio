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
    }
}