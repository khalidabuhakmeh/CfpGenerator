using Microsoft.AspNetCore.Http;

namespace CfpGenerator.Models
{
    public static class HtmxExtensions
    {
        public static bool IsHtmx(this HttpRequest request)
        {
            return request.Headers.ContainsKey("HX-Request");
        }
    }
}