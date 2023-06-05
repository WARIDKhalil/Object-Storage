using API.Requests;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using MinioMiddleware.Requests;
using MinioMiddleware.Responses;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContentController : ControllerBase
    {
        private readonly ContentService _contentService;

        public ContentController(ContentService contentService)
        {
            _contentService = contentService;
        }

        [HttpPost]
        [Route("upload")]
        [ProducesResponseType(typeof(UploadContentSuccessResponse), 200)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> Upload([FromForm] UploadContentApiRequest request)
        {
            try
            {
                return Ok(await _contentService.AddContentAsync(request));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("download")]
        [ProducesResponseType(typeof(FileContentResult), 200)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> Download([FromBody] DownloadContentRequest request)
        {
            try
            {
                DownloadContentSuccessResponse response = await _contentService.DownloadContentAsync(request);
                return File(response.Bytes, response.Content.MimeType, fileDownloadName : response.Content.OriginalName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
