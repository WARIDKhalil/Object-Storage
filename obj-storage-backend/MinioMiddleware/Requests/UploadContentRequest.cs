using Microsoft.AspNetCore.Http;

namespace MinioMiddleware.Requests
{
    public class UploadContentRequest
    {
        public string BucketName { get; set; }
        public Guid ContentId { get; set; }
        public IFormFile Content { get; set; }
    }
}
