using System.Text.Encodings.Web;
using System.Text.Unicode;
using KhedmatShomar.Data;
using KhedmatShomar.Middleware;
using KhedmatShomar.Services;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// بدون این تنظیم، متن فارسی داخل attributeها (title، description، og و...) به شکل &#x62E; رمزگذاری می‌شود
builder.Services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.All));
builder.Services.AddSingleton<ServiceCalculator>();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Default")
                      ?? "Data Source=khedmatshomar.db"));

var app = builder.Build();

// جدول لاگ بازدید — بدون نیاز به Migration برای این یک جدول ساده
using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.EnsureCreated();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// صفحه ۴۰۴ سفارشی که Status Code واقعی را حفظ می‌کند (نه ۲۰۰)
app.UseStatusCodePagesWithReExecute("/Home/Status", "?code={0}");

app.UseHttpsRedirection();

// بدون charset=utf-8 مرورگر متن فارسی فایل‌های .txt (مثل llms.txt) را ناخوانا نشان می‌دهد
var contentTypes = new FileExtensionContentTypeProvider();
contentTypes.Mappings[".txt"] = "text/plain; charset=utf-8";
app.UseStaticFiles(new StaticFileOptions { ContentTypeProvider = contentTypes });
app.UseRouting();

app.UseMiddleware<VisitLoggingMiddleware>();

app.UseAuthorization();

app.MapStaticAssets();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
