using Domain.Aggregates.Content;

namespace API.Responses
{
    public class DownloadContentSuccessResponse
    {
        public Content Content { get; set; }
        public byte[] Bytes { get; set; }
    }
}
