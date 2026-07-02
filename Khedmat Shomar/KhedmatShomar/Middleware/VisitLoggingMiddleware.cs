using System.Security.Cryptography;
using System.Text;
using KhedmatShomar.Data;

namespace KhedmatShomar.Middleware;

/// <summary>
/// هر بازدید صفحه (ناوبری GET، نه فایل‌های استاتیک) را در SQLite ثبت می‌کند.
/// خطای لاگ نباید پاسخ کاربر را خراب کند.
/// </summary>
public class VisitLoggingMiddleware(RequestDelegate next, ILogger<VisitLoggingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        if (ShouldLog(context))
        {
            try
            {
                var db = context.RequestServices.GetRequiredService<AppDbContext>();
                db.VisitLogs.Add(new VisitLog
                {
                    VisitedAtUtc = DateTime.UtcNow,
                    Path = context.Request.Path.Value ?? "/",
                    IpHash = HashIp(context.Connection.RemoteIpAddress?.ToString() ?? ""),
                    UserAgent = Truncate(context.Request.Headers.UserAgent.ToString(), 512)
                });
                await db.SaveChangesAsync(context.RequestAborted);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "ثبت بازدید ناموفق بود");
            }
        }

        await next(context);
    }

    private static bool ShouldLog(HttpContext context)
    {
        if (!HttpMethods.IsGet(context.Request.Method))
            return false;
        var path = context.Request.Path.Value ?? "/";
        // فقط صفحات؛ فایل‌های استاتیک (پسونددار) و service worker شمرده نمی‌شوند
        return !Path.HasExtension(path);
    }

    private static string HashIp(string ip)
        => Convert.ToHexStringLower(SHA256.HashData(Encoding.UTF8.GetBytes(ip)));

    private static string Truncate(string s, int max) => s.Length <= max ? s : s[..max];
}
