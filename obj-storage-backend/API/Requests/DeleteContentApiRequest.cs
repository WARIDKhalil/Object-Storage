namespace API.Requests
{
    public class DeleteContentApiRequest
    {
        public string BucketName { get; set; }
        public Guid ContentId { get; set; }
    }
}
