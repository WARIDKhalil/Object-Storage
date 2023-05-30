﻿using Microsoft.AspNetCore.Mvc;
using Midleware;
using Midleware.Responses;
using Minio;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IMiddlewareService _middlewareService;

        public FileController(IMiddlewareService midlewareService)
        {
            _middlewareService = midlewareService;
        }

        [HttpPost]
        [Route("upload")]
        [ProducesResponseType(typeof(UploadSuccessResponse), 200)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            try
            {
                return Ok( await _middlewareService.Upload(Guid.NewGuid(), file) );
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("download/{id}")]
        [ProducesResponseType(typeof(FileContentResult), 200)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> Download([FromRoute] Guid id)
        {
            try
            {
                var bytes = await _middlewareService.Download(id);
                return File(bytes, Helper.GetMimeType("png"), fileDownloadName : "test.png");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}