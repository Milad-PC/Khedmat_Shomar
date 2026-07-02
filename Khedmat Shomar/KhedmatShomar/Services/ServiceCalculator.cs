using System.Globalization;
using KhedmatShomar.Models;

namespace KhedmatShomar.Services;

public class ServiceCalculator
{
    public const int BaseMonths = 24;

    private static readonly PersianCalendar Pc = new();

    /// <summary>
    /// تاریخ اعزام شمسی + دوره پایه ۲۴ ماه، منهای کسری‌ها، به‌علاوه اضافه‌خدمت.
    /// برخلاف نسخه قدیمی، غیبت و متفرقه «روز» اضافه می‌کنند نه ماه×۳.
    /// </summary>
    public (DateTime Start, DateTime Finish) Calculate(Soldier s)
    {
        var start = Pc.ToDateTime(s.Sal!.Value, s.Mah, s.Roz, 0, 0, 0, 0);

        var finish = start.AddMonths(BaseMonths);
        finish = finish.AddDays(-s.RozKasri).AddMonths(-s.MahKasri);
        finish = finish.AddDays(-s.RozBasij).AddMonths(-s.MahBasij);
        if (s.Taahol == 2)
            finish = finish.AddMonths(-2);
        finish = finish.AddMonths(-3 * s.Bache);
        finish = finish.AddDays(s.RozFarar + s.RozEzaf);

        return (start, finish);
    }

    /// <summary>روز معتبر برای ماه/سال شمسی داده‌شده؟ (مثلاً ۳۱ اسفند سال غیرکبیسه نامعتبر است)</summary>
    public bool IsValidShamsiDate(int year, int month, int day)
        => day >= 1 && month >= 1 && month <= 12 && day <= Pc.GetDaysInMonth(year, month);
}
