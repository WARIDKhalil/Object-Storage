namespace MinioMiddleware.Helpers
{
    public static class ContentHelper
    {
        private static readonly Dictionary<string, string> MimeTypes;

        static ContentHelper()
        {
            MimeTypes = new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"},
                {".mp3", "audio/mp3"},
                {".mp4", "video/mp4"}
            };
        }

        public static string GetMimeType(string extension)
        {
            return MimeTypes[extension];
        }

        public static byte[] ExtractBytes<T>(T obj) where T : Stream
        {
            MemoryStream ms = new();
            obj.CopyTo(ms);
            return ms.ToArray();
        }
    }
}
