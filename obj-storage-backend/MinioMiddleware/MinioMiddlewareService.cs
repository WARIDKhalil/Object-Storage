﻿using Microsoft.Extensions.Options;
using Minio;
using MinioMiddleware.Helpers;
using MinioMiddleware.Requests;
using MinioMiddleware.Responses;
using MinioMiddleware.Settings;

namespace MinioMiddleware
{
    public class MinioMiddlewareService
    {
        private readonly MinIOSettings _options;
        private readonly MinioClient _client;

        public MinioMiddlewareService(IOptions<MinIOSettings> options)
        {
            _options = options.Value ?? throw new ArgumentNullException(nameof(options));
            _client = new MinioClient().WithEndpoint(_options.EndPoint)
                                       .WithCredentials(_options.Username, _options.Password)
                                       .WithSSL(_options.SupportHttps)
                                       .Build();
        }

        public async Task<UploadContentSuccessResponse> UploadContentAsync(UploadContentRequest request)
        {
            var args = new PutObjectArgs().WithBucket(request.BucketName)
                                      .WithObject(request.ContentId.ToString())
                                      .WithStreamData(request.Content.OpenReadStream())
                                      .WithContentType(request.Content.ContentType)
                                      .WithObjectSize(request.Content.Length);

            await _client.PutObjectAsync(args);
            return new UploadContentSuccessResponse { ContentId = request.ContentId, Message = "Content was uploaded successfully to MinIO" };
        }

        public async Task<byte[]> DownloadContentAsync(DownloadContentRequest request)
        {
            byte[]? buffer = null;
            GetObjectArgs getObjectArgs = new GetObjectArgs().WithBucket(request.BucketName)
                                                        .WithObject(request.ContentId.ToString())
                                                        .WithCallbackStream((stream) =>
                                                        {
                                                            buffer = ContentHelper.ExtractBytes(stream);
                                                        });
            await _client.GetObjectAsync(getObjectArgs);
            return buffer ?? Array.Empty<byte>(); ;
        }

        public async Task<byte[]> StreamContentAsync(StreamContentRequest request)
        {
            byte[]? buffer = null;
            GetObjectArgs getObjectArgs = new GetObjectArgs().WithBucket(request.BucketName)
                                                        .WithObject(request.ContentId.ToString())
                                                        .WithOffsetAndLength(request.Offset, Constants.Constants.CHUNK_SIZE)
                                                        .WithCallbackStream((stream) =>
                                                        {
                                                            buffer = ContentHelper.ExtractBytes(stream);
                                                        });
            await _client.GetObjectAsync(getObjectArgs);
            return buffer ?? Array.Empty<byte>();
        }

        public async Task<DeleteContentSuccessResponse> DeleteContentAsync(DeleteContentRequest request)
        {
            RemoveObjectArgs removeObjectArgs = new RemoveObjectArgs().WithBucket(request.BucketName)
                                                                .WithObject(request.ContentId.ToString());
            await _client.RemoveObjectAsync(removeObjectArgs);
            return new() { ContentId= request.ContentId, Message = "Content was deleted successfully from MinIO" };
        }   
    }
}
