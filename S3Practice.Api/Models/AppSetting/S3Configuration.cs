namespace S3Practice.Api.Models.AppSetting
{
    public class S3Configuration
    {
        public string AccessKey { get; set; } = string.Empty;
        public string SecretKey { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public string ServiceUrl { get; set; } = string.Empty;
        public string BucketName { get; set; } = string.Empty;
       

    }
}
