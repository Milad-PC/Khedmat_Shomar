namespace KhedmatShomar.Models;

/// <summary>
/// متادیتای سئوی یک صفحه. هر View مقادیر مخصوص خودش را ست می‌کند و بقیه
/// از زنجیره Fallback پر می‌شوند، تا خروجی head هیچ‌وقت خالی نماند.
/// </summary>
public sealed class SeoMetadata
{
    /// <summary>دامنه اصلی سایت؛ مبنای ساخت URLهای مطلق (canonical و og:image).</summary>
    public const string BaseUrl = "https://khedmatshomar.ir";

    public const string SiteName = "خدمت‌شمار";

    private const string FallbackTitle = "خدمت شمار | محاسبه پایان خدمت سربازی و شمارش معکوس ترخیص";

    private const string FallbackDescription =
        "خدمت شمار آنلاین و رایگان: تاریخ اعزام و کسری‌هایت را وارد کن تا تاریخ دقیق ترخیص و " +
        "شمارش معکوس روزهای باقی‌مانده از خدمت سربازی را ببینی. بدون ثبت‌نام و قابل نصب روی گوشی.";

    private const string FallbackImagePath = "/icons/icon-512.png";

    /// <summary>عنوان کوتاه صفحه؛ به شکل «{Heading} | خدمت‌شمار» تبدیل به title می‌شود.</summary>
    public string? Heading { get; set; }

    /// <summary>عنوان کامل title؛ اگر ست شود، Heading نادیده گرفته می‌شود.</summary>
    public string? Title { get; set; }

    public string? Description { get; set; }

    /// <summary>
    /// مسیر نسبی صفحه برای ساخت canonical مطلق؛ بدون query string.
    /// null یعنی صفحه URL معناداری ندارد (مثل ۴۰۴) و اصلاً canonical نگیرد —
    /// canonical اشتباه به صفحه اصلی بدتر از نبودن آن است.
    /// </summary>
    public string? Path { get; set; } = "/";

    public bool Index { get; set; } = true;

    public bool Follow { get; set; } = true;

    public string? OgTitle { get; set; }

    public string? OgDescription { get; set; }

    public string ImagePath { get; set; } = FallbackImagePath;

    public string ImageAlt { get; set; } = "لوگوی خدمت‌شمار";

    public string OgType { get; set; } = "website";

    /// <summary>JSON-LD آماده برای درج در صفحه؛ فقط برای صفحات قابل ایندکس.</summary>
    public string? JsonLd { get; set; }

    public string FinalTitle => Title
        ?? (string.IsNullOrWhiteSpace(Heading) ? FallbackTitle : $"{Heading} | {SiteName}");

    public string FinalDescription => Description ?? FallbackDescription;

    public string FinalOgTitle => OgTitle ?? Heading ?? FinalTitle;

    public string FinalOgDescription => OgDescription ?? FinalDescription;

    public string? CanonicalUrl => Path is null ? null : Absolute(Path);

    public string ImageUrl => Absolute(ImagePath);

    /// <summary>
    /// دستور robots صفحه. برای صفحات قابل ایندکس، محدودیت Snippet و پیش‌نمایش تصویر هم برداشته می‌شود.
    /// </summary>
    public string Robots
    {
        get
        {
            var directives = $"{(Index ? "index" : "noindex")}, {(Follow ? "follow" : "nofollow")}";
            return Index ? $"{directives}, max-snippet:-1, max-image-preview:large" : directives;
        }
    }

    private static string Absolute(string path)
        => path.StartsWith("http", StringComparison.OrdinalIgnoreCase)
            ? path
            : $"{BaseUrl}/{path.TrimStart('/')}";
}
