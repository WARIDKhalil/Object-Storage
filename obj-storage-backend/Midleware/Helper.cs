using Microsoft.AspNetCore.Http;

namespace Midleware
{
    public static class Helper
    {
        public static byte[] GetBytes(IFormFile file)
        {
            MemoryStream ms = new();
            file.CopyTo(ms);
            return ms.ToArray();
        }
    }
}
