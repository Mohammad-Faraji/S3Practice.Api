namespace S3Practice.Api.Service.Interface
{
    public interface IFileStorageService
    {
        Task<string> UploadAsync(IFormFile file, string name);
        Task<Stream> DownloadAsync(string fileUrl, CancellationToken ct);
        Task DeleteAsync(string fileUrl);
    }
}
