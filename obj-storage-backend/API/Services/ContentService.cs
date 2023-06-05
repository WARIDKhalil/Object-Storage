using API.Requests;
using Domain.Aggregates.Content;
using Domain.Contracts;
using MinioMiddleware;
using MinioMiddleware.Helpers;
using MinioMiddleware.Requests;
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

        public async Task<DownloadContentSuccessResponse> DownloadContentAsync(DownloadContentRequest request)
        {
            Content content = await _repository.GetByIdAsync(request.ContentId);
            byte[] bytes = await _minioService.DownloadContentAsync(request);
            return new() { Content = content, Bytes = bytes };
        }

    }
}
