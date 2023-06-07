namespace API.Requests
{
    public class StreamContentApiRequest
    {
        public string BucketName { get; set; }
        public Guid ContentId { get; set; }
    }
}
