using API.Requests;
using API.Responses;
using API.Services;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> UploadAsync([FromForm] UploadContentApiRequest request)
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
        [Route("download/{bucket}/{id}")]
        [ProducesResponseType(typeof(FileContentResult), 200)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> DownloadAsync([FromRoute] string bucket, [FromRoute] Guid id)
        {
            try
            {
                DownloadContentSuccessResponse response = await _contentService.DownloadContentAsync(new()
                {
                    BucketName = bucket,
                    ContentId = id
                });
                return File(response.Bytes, response.Content.MimeType, fileDownloadName : response.Content.OriginalName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("stream/{bucket}/{id}")]
        public async Task StreamAsync([FromRoute] string bucket, [FromRoute] Guid id)
        {
            StreamContentSuccessResponse response = await _contentService.StreamContentAsync(new()
            {
                BucketName = bucket,
                ContentId = id
            }, Request);

            Response.StatusCode = 206;
            Response.Headers["Accept-Ranges"] = "bytes";
            Response.Headers["Content-Range"] = string.Format($" bytes {response.Offset}-{response.ContentSize - 1}/{response.ContentSize}");
            Response.ContentType = response.MimeType;
            Stream outputStream = Response.Body;
            await outputStream.WriteAsync(response.Chunk.AsMemory(0, response.Chunk.Length));
        }

        [HttpDelete]
        [Route("{bucket}/{id}")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> DeleteAsync([FromRoute] string bucket, [FromRoute] Guid id)
        {
            try
            {
                await _contentService.DeleteContentAsync(new()
                {
                    BucketName = bucket,
                    ContentId = id
                });
                return Ok($"Content of Id : {id} was successfully deleted");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
