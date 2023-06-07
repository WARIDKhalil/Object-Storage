namespace API.Responses
{
    public class StreamContentSuccessResponse
    {
        public string MimeType { get; set; }
        public long ContentSize { get; set; }
        public long Offset { get; set; }
        public byte[] Chunk { get; set; }
    }
}
