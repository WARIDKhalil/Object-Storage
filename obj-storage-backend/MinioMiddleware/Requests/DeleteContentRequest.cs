namespace MinioMiddleware.Requests
{
    public class DeleteContentRequest
    {
        public string BucketName { get; set; }
        public Guid ContentId { get; set; }
    }
}
