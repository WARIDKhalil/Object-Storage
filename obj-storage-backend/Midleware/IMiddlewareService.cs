﻿using Microsoft.AspNetCore.Http;
using Midleware.Responses;

namespace Midleware
{
    public interface IMiddlewareService
    {
        Task<UploadSuccessResponse> Upload(Guid id, IFormFile file);
        Task<byte[]> Download(Guid id);
    }
}
