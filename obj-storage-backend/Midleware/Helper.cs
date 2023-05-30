using Microsoft.AspNetCore.Http;

namespace Midleware
{
    public static class Helper
    {
        private static readonly Dictionary<string, string> MimeTypes;

        static Helper()
        {
            MimeTypes = new Dictionary<string, string>
            {
                {"txt", "text/plain"},
                {"pdf", "application/pdf"},
                {"doc", "application/vnd.ms-word"},
                {"docx", "application/vnd.ms-word"},
                {"xls", "application/vnd.ms-excel"},
                {"xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
                {"png", "image/png"},
                {"jpg", "image/jpeg"},
                {"jpeg", "image/jpeg"},
                {"gif", "image/gif"},
                {"csv", "text/csv"}
            };
        }

        public static byte[] GetBytes(IFormFile file)
        {
            MemoryStream ms = new();
            file.CopyTo(ms);
            return ms.ToArray();
        }

        public static string GetMimeType(string extension)
        {
            return MimeTypes[extension];
        }
    }
}
