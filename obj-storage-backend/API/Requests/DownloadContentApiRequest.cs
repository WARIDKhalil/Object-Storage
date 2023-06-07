namespace API.Requests
{
    public class DownloadContentApiRequest
    {
        public string BucketName { get; set; }
        public Guid ContentId { get; set; }
    }
}
