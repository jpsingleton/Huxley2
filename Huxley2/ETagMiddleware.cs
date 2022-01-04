// Original: https://gist.github.com/madskristensen/36357b1df9ddbfd123162cd4201124c4 - Apache 2.0 license
// Changes by James Singleton - EUPL-1.2
// https://joinup.ec.europa.eu/collection/eupl/matrix-eupl-compatible-open-source-licences#section-2

using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;

public class ETagMiddleware
{
    private readonly RequestDelegate _next;

    public ETagMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var response = context.Response;
        var originalStream = response.Body;

        using (var ms = new MemoryStream())
        {
            response.Body = ms;

            await _next(context);

            if (IsEtagSupported(response))
            {
                // If etag is already set then we should use it
                if (!response.Headers.TryGetValue(HeaderNames.ETag, out var checksum))
                {
                    // Otherwise generate our own
                    checksum = CalculateChecksum(ms);
                    response.Headers[HeaderNames.ETag] = checksum;
                }

                if (context.Request.Headers.TryGetValue(HeaderNames.IfNoneMatch, out var etag) && checksum == etag)
                {
                    response.StatusCode = StatusCodes.Status304NotModified;
                    return;
                }
            }

            ms.Position = 0;
            await ms.CopyToAsync(originalStream);
        }
    }

    private static bool IsEtagSupported(HttpResponse response)
    {
        if (response.StatusCode != StatusCodes.Status200OK)
            return false;

        // If etag is already set then we should handle it
        if (response.Headers.ContainsKey(HeaderNames.ETag))
            return true;

        if (response.Body.Length > 1024 * 1024) // 1MB
            return false;

        return true;
    }

    private static string CalculateChecksum(MemoryStream ms)
    {
        var checksum = "";

        using (var algo = SHA256.Create())
        {
            ms.Position = 0;
            byte[] bytes = algo.ComputeHash(ms);
            checksum = $"\"{WebEncoders.Base64UrlEncode(bytes)}\"";
        }

        return checksum;
    }
}

public static class ApplicationBuilderExtensions
{
    public static void UseETagger(this IApplicationBuilder app)
    {
        app.UseMiddleware<ETagMiddleware>();
    }
}
