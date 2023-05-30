using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Midleware.Responses;
using Minio;

namespace Midleware
{
    public class MiddlewareService : IMiddlewareService
    {
        private readonly string BUCKET = "bucket";
        private readonly MinIOSettings _options;
        private readonly MinioClient _client;

        public MiddlewareService(IOptions<MinIOSettings> options)
        {
            _options = options.Value ?? throw new Exception($"{nameof(MinIOSettings)} is undefined");
            _client = new MinioClient().WithEndpoint(_options.EndPoint)
                                       .WithCredentials(_options.Username, _options.Password)
                                       .WithSSL(_options.SupportHttps)
                                       .Build();
        }

        public async Task<byte[]> Download(Guid id)
        {
            byte[]? buffer = null; 
            GetObjectArgs getObjectArgs = new GetObjectArgs().WithBucket(BUCKET)
                                                        .WithObject(id.ToString())
                                                        .WithCallbackStream((stream) =>
                                                        {
                                                            MemoryStream ms = new();
                                                            stream.CopyTo(ms);
                                                            buffer = ms.ToArray();
                                                        });
            await _client.GetObjectAsync(getObjectArgs);

            if (buffer == null)
            {
                return Array.Empty<byte>();
            }

            return buffer;
        }

        public async Task<UploadSuccessResponse> Upload(Guid id, IFormFile file)
        {
            var args = new PutObjectArgs().WithBucket(BUCKET)
                                      .WithObject(id.ToString())
                                      .WithStreamData(new MemoryStream(Helper.GetBytes(file)))
                                      .WithContentType(file.ContentType)
                                      .WithObjectSize(file.Length);

            await _client.PutObjectAsync(args);
            return new UploadSuccessResponse { FileId = id, Message = "File was uploaded successfully" };
        }

    }
}
