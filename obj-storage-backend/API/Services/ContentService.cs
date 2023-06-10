using API.Requests;
using API.Responses;
using Domain.Aggregates.Content;
using Domain.Contracts;
using MinioMiddleware;
using MinioMiddleware.Helpers;
using MinioMiddleware.Responses;

namespace API.Services
{
    public class ContentService
    {
        private readonly IRepository<Content> _repository;
        private readonly MinioMiddlewareService _minioService;

        public ContentService(IRepository<Content> repository, MinioMiddlewareService minioService)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _minioService = minioService ?? throw new ArgumentNullException(nameof(minioService));
        }

        public async Task<UploadContentSuccessResponse> AddContentAsync(UploadContentApiRequest request)
        {
            string extension = Path.GetExtension(request.Content.FileName);
            Content content = new()
            {
                CreatedAt = DateTime.Now,
                OriginalName = request.Content.FileName,
                Size = request.Content.Length,
                Extension = extension,
                MimeType = ContentHelper.GetMimeType(extension)
            };
            await _repository.AddAsync(content);
            return await _minioService.UploadContentAsync( new() 
            {
                BucketName = request.BucketName, 
                Content = request.Content, 
                ContentId = content.Id 
            });
        }

        public async Task<DownloadContentSuccessResponse> DownloadContentAsync(DownloadContentApiRequest request)
        {
            Content content = await _repository.GetByIdAsync(request.ContentId);
            byte[] bytes = await _minioService.DownloadContentAsync(new()
            {
                BucketName = request.BucketName,
                ContentId = request.ContentId
            });
            return new() { Content = content, Bytes = bytes };
        }

        public async Task<StreamContentSuccessResponse> StreamContentAsync(StreamContentApiRequest request, HttpRequest context)
        {
            Content content = await _repository.GetByIdAsync(request.ContentId);
            long offset = 0;
            if (!string.IsNullOrEmpty(context.Headers["Range"]))
            {
                string[] range = context.Headers["Range"].ToString().Split(new char[] { '=', '-' });
                offset = long.Parse(range[1]);
            }
            byte[] chunk = await _minioService.StreamContentAsync(new()
            {
                BucketName = request.BucketName,
                ContentId = request.ContentId,
                Offset = offset
            });
            return new() { Chunk= chunk, ContentSize = content.Size, Offset = offset, MimeType = content.MimeType };
        }

        public async Task DeleteContentAsync(DeleteContentApiRequest request)
        {
            await _repository.DeleteByIdAsync(request.ContentId);
            await _minioService.DeleteContentAsync(new() 
            { 
                ContentId = request.ContentId, 
                BucketName = request.BucketName 
            });
        }

    }
}
