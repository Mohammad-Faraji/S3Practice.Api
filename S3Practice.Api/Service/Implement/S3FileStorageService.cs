using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;
using S3Practice.Api.Models.AppSetting;
using S3Practice.Api.Service.Interface;

namespace S3Practice.Api.Service.Implement
{
    public class S3FileStorageService : IFileStorageService
    {
        private readonly IAmazonS3 _s3Client;
        private readonly S3Configuration _config;

        public S3FileStorageService(IOptions<S3Configuration> configOptions)
        {
            _config = configOptions.Value;

            var credentials = new BasicAWSCredentials(_config.AccessKey, _config.SecretKey);

            var s3Config = new AmazonS3Config
            {
                RegionEndpoint = Amazon.RegionEndpoint.GetBySystemName(_config.Region),
                ServiceURL = _config.ServiceUrl,
                ForcePathStyle = true // Required for some S3-compatible services برای آروان/MinIO لازم میشه
            };

            _s3Client = new AmazonS3Client(credentials, s3Config);

        }


        // ساخت آدرس کامل فایل (URL)
        private string GenerateS3Url(string key)
        {
            return $"{_config.ServiceUrl}/{_config.BucketName.ToLower()}/{key}";
        }


        private string GetKeyFromUrl(string fileUrl)
        {
            var prefix = $"{_config.ServiceUrl}/{_config.BucketName.ToLower()}/";

            if (fileUrl.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            {
                return fileUrl.Substring(prefix.Length);
            }

            throw new ArgumentException("URL is not valid for this bucket.", nameof(fileUrl));
        }

        public Task DeleteAsync(string fileUrl)
        {
            throw new NotImplementedException();
        }

        public Task<Stream> DownloadAsync(string fileUrl, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async Task<string> UploadAsync(IFormFile file, string name)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentNullException("File is empty", nameof(file));

            var key = $"{name.Replace(" " , "-")}_{Guid.NewGuid()}{Path.GetExtension(file.Name)}";

            using var stearm = file.OpenReadStream();

            var request = new PutObjectRequest
            {
                BucketName = _config.BucketName.ToLower(),
                Key = key,
                InputStream = stearm,
                ContentType = file.ContentType,
                CannedACL = S3CannedACL.Private // فایل private ذخیره می‌شود
            };

            await _s3Client.PutObjectAsync(request);

            return GenerateS3Url(key);
        }
    }
}
