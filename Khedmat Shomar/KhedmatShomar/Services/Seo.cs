using KhedmatShomar.Models;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace KhedmatShomar.Services;

public static class Seo
{
    private const string ViewDataKey = "__Seo";

    public static void SetSeo(this ViewDataDictionary viewData, SeoMetadata seo) => viewData[ViewDataKey] = seo;

    /// <summary>متادیتای ست‌شده توسط View؛ اگر View چیزی ست نکرده بود، پیش‌فرض سایت.</summary>
    public static SeoMetadata GetSeo(this ViewDataDictionary viewData)
        => viewData[ViewDataKey] as SeoMetadata ?? new SeoMetadata();

    /// <summary>
    /// JSON-LD صفحه اصلی: خود سایت، اپلیکیشن و ناشر آن.
    /// فقط چیزهایی که روی صفحه واقعاً وجود دارند اینجا اعلام شده‌اند.
    /// </summary>
    public const string HomeJsonLd = $$"""
    {
      "@context": "https://schema.org",
      "@graph": [
        {
          "@type": "WebSite",
          "@id": "{{SeoMetadata.BaseUrl}}/#website",
          "url": "{{SeoMetadata.BaseUrl}}/",
          "name": "خدمت‌شمار",
          "alternateName": ["خدمت شمار", "خدمت شمار سربازی"],
          "description": "محاسبه پایان خدمت سربازی و شمارش معکوس روزهای باقی‌مانده",
          "inLanguage": "fa-IR",
          "publisher": { "@id": "{{SeoMetadata.BaseUrl}}/#publisher" }
        },
        {
          "@type": "WebApplication",
          "@id": "{{SeoMetadata.BaseUrl}}/#app",
          "url": "{{SeoMetadata.BaseUrl}}/",
          "name": "خدمت‌شمار",
          "alternateName": "خدمت شمار",
          "description": "با وارد کردن تاریخ اعزام و کسری‌ها، تاریخ ترخیص و شمارش معکوس روزهای باقی‌مانده از خدمت سربازی را نشان می‌دهد.",
          "applicationCategory": "UtilitiesApplication",
          "operatingSystem": "Any",
          "browserRequirements": "نیازمند مرورگر با پشتیبانی از JavaScript",
          "inLanguage": "fa-IR",
          "isAccessibleForFree": true,
          "featureList": [
            "محاسبه تاریخ پایان خدمت بر اساس تاریخ اعزام شمسی",
            "اعمال کسری پدر، کسری بسیج، تاهل و فرزند",
            "احتساب غیبت و اضافه‌خدمت",
            "شمارش معکوس زنده روزهای باقی‌مانده",
            "نصب روی گوشی به‌صورت اپلیکیشن"
          ],
          "offers": {
            "@type": "Offer",
            "price": "0",
            "priceCurrency": "IRR"
          },
          "publisher": { "@id": "{{SeoMetadata.BaseUrl}}/#publisher" }
        },
        {
          "@type": "Organization",
          "@id": "{{SeoMetadata.BaseUrl}}/#publisher",
          "name": "لایت کمپانی",
          "url": "https://lightcompany.ir/",
          "logo": "{{SeoMetadata.BaseUrl}}/icons/icon-512.png"
        }
      ]
    }
    """;
}
