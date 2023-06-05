namespace API.Requests
{
    public class UploadContentApiRequest
    {
        public string BucketName { get; set; }
        public IFormFile Content { get; set; }
    }
}
