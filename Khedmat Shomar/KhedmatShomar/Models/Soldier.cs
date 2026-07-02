using System.ComponentModel.DataAnnotations;

namespace KhedmatShomar.Models;

public class Soldier
{
    [Required(ErrorMessage = "سال اعزام را وارد کن")]
    [Range(1380, 1500, ErrorMessage = "سال باید بین ۱۳۸۰ تا ۱۵۰۰ باشد")]
    public int? Sal { get; set; }

    [Range(1, 12, ErrorMessage = "ماه نامعتبر است")]
    public int Mah { get; set; } = 1;

    [Range(1, 31, ErrorMessage = "روز نامعتبر است")]
    public int Roz { get; set; } = 1;

    [Range(0, 1000, ErrorMessage = "کسری پدر (روز) نامعتبر است")]
    public int RozKasri { get; set; }

    [Range(0, 24, ErrorMessage = "کسری پدر (ماه) نامعتبر است")]
    public int MahKasri { get; set; }

    [Range(0, 1000, ErrorMessage = "کسری بسیج (روز) نامعتبر است")]
    public int RozBasij { get; set; }

    [Range(0, 24, ErrorMessage = "کسری بسیج (ماه) نامعتبر است")]
    public int MahBasij { get; set; }

    /// <summary>0 = مجرد، 2 = متاهل (مطابق فرم قدیمی)</summary>
    [Range(0, 2, ErrorMessage = "وضعیت تاهل نامعتبر است")]
    public int Taahol { get; set; }

    [Range(0, 20, ErrorMessage = "تعداد فرزند نامعتبر است")]
    public int Bache { get; set; }

    [Range(0, 2000, ErrorMessage = "غیبت (روز) نامعتبر است")]
    public int RozFarar { get; set; }

    [Range(0, 2000, ErrorMessage = "اضافه‌خدمت متفرقه (روز) نامعتبر است")]
    public int RozEzaf { get; set; }
}
