using System.Diagnostics;
using System.Globalization;
using KhedmatShomar.Models;
using KhedmatShomar.Services;
using Microsoft.AspNetCore.Mvc;

namespace KhedmatShomar.Controllers;

public class HomeController(ServiceCalculator calculator) : Controller
{
    private const string CookieName = "KhedmatShomar";

    public IActionResult Index()
    {
        if (TryReadCookie(out _, out _))
            return RedirectToAction(nameof(Counter));
        return View(new Soldier());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ShowFinish(Soldier input)
    {
        if (ModelState.IsValid && !calculator.IsValidShamsiDate(input.Sal!.Value, input.Mah, input.Roz))
            ModelState.AddModelError(nameof(input.Roz), "این روز در ماه انتخابی وجود ندارد");

        if (!ModelState.IsValid)
            return View(nameof(Index), input);

        var (start, finish) = calculator.Calculate(input);
        WriteCookie(start, finish);

        return RedirectToAction(nameof(Counter));
    }

    public IActionResult Counter()
    {
        if (!TryReadCookie(out var start, out var finish))
            return RedirectToAction(nameof(Index));

        var remain = Math.Max(0, (int)Math.Ceiling((finish - DateTime.Now).TotalDays));
        var totalDays = Math.Max(1.0, (finish - start).TotalDays);
        var pct = (int)Math.Round(Math.Clamp(100 * (1 - (finish - DateTime.Now).TotalDays / totalDays), 0, 100));
        var (msg, color) = MessageFor(remain);

        return View(new CounterViewModel
        {
            Start = start,
            Finish = finish,
            RemainDays = remain,
            PercentDone = pct,
            Message = msg,
            MessageColor = color,
            DischargeShamsi = finish.ToShamsi(),
            Years = remain / 365,
            Months = remain % 365 / 30,
            Days = remain % 365 % 30
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Recalc()
    {
        Response.Cookies.Delete(CookieName);
        return RedirectToAction(nameof(Index));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
        => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

    private static (string Text, string Color) MessageFor(int d) => d switch
    {
        <= 0 => ("داداش ول کن ، خدمتت تموم شده!", "good"),
        <= 20 => ("دیگه وقتشه نبود بکشی", "good"),
        <= 99 => ("دو رقمی شدی", "good"),
        <= 200 => ("پایه بالا شدی", "mid"),
        <= 360 => ("کمر خدمت شکست", "mid"),
        <= 430 => ("صفرت باز شد", "warn"),
        <= 550 => ("تا سال بوق خدمت داری", "warn"),
        <= 670 => ("اندک ماه", "far"),
        _ => ("کو تا آموزشی تموم شه", "far")
    };

    private bool TryReadCookie(out DateTime start, out DateTime finish)
    {
        start = finish = default;
        var raw = Request.Cookies[CookieName];
        if (string.IsNullOrEmpty(raw))
            return false;

        var parts = raw.Split('|');
        if (parts.Length != 2
            || !DateTime.TryParse(parts[0], CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out finish)
            || !DateTime.TryParse(parts[1], CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out start))
            return false;

        return true;
    }

    private void WriteCookie(DateTime start, DateTime finish)
    {
        var value = $"{finish:O}|{start:O}";
        Response.Cookies.Append(CookieName, value, new CookieOptions
        {
            Expires = DateTimeOffset.Now.AddDays(90),
            IsEssential = true,
            SameSite = SameSiteMode.Lax
        });
    }
}
