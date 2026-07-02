using System.Globalization;

namespace KhedmatShomar.Services;

public static class PersianDate
{
    public static readonly string[] MonthNames =
    [
        "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور",
        "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند"
    ];

    private static readonly PersianCalendar Pc = new();

    public static string ToPersianDigits(this string s)
    {
        const string fa = "۰۱۲۳۴۵۶۷۸۹";
        var chars = s.ToCharArray();
        for (var i = 0; i < chars.Length; i++)
        {
            if (chars[i] is >= '0' and <= '9')
                chars[i] = fa[chars[i] - '0'];
        }
        return new string(chars);
    }

    public static string ToPersianDigits(this int n) => n.ToString(CultureInfo.InvariantCulture).ToPersianDigits();

    /// <summary>مثل «۱۲ آبان ۱۴۰۵»</summary>
    public static string ToShamsi(this DateTime d)
    {
        var day = Pc.GetDayOfMonth(d);
        var month = Pc.GetMonth(d);
        var year = Pc.GetYear(d);
        return $"{day.ToPersianDigits()} {MonthNames[month - 1]} {year.ToPersianDigits()}";
    }
}
