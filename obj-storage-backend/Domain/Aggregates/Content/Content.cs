using Domain.Shared;

namespace Domain.Aggregates.Content
{
    public class Content : BaseEntity
    {
        public string OriginalName { get; set; }
        public string Extension { get; set; }
        public string MimeType { get; set; }
        public long Size { get; set; }

    }
}
