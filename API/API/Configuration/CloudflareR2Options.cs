namespace API.Configuration
{
    public class CloudflareR2Options
    {
        public const string SectionName = "CloudflareR2";

        public string AccessKey { get; set; } = string.Empty;
        public string SecretKey { get; set; } = string.Empty;
        public string Endpoint { get; set; } = string.Empty;
        public string BucketName { get; set; } = string.Empty;
        public string PublicAccessUrl { get; set; } = string.Empty;
    }
}
