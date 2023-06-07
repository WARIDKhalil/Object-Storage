namespace MinioMiddleware.Requests
{
    public class StreamContentRequest
    {
        public string BucketName { get; set; }
        public Guid ContentId { get; set; }
        public long Offset { get; set; }
    }
}
