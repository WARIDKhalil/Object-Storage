namespace MinioMiddleware.Requests
{
    public class DownloadContentRequest
    {
        public string BucketName { get; set; }
        public Guid ContentId { get; set; }
    }
}
