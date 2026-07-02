namespace KhedmatShomar.Models;

public class CounterViewModel
{
    public DateTime Start { get; set; }
    public DateTime Finish { get; set; }
    public int RemainDays { get; set; }
    public int PercentDone { get; set; }
    public string Message { get; set; } = "";
    /// <summary>نام متغیر CSS رنگ پیام: good / mid / warn / far</summary>
    public string MessageColor { get; set; } = "good";
    public string DischargeShamsi { get; set; } = "";
    public int Years { get; set; }
    public int Months { get; set; }
    public int Days { get; set; }
}
