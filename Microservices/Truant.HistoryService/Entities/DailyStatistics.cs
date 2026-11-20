namespace Truant.HistoryService.Entities;

public class DailyStatistics
{
    public long Id { get; set; }
    public long SemesterId { get; set; }
    public int Day { get; set; }
    public int TotalPleasure { get; set; }
    public int Outcome { get; set; } 

    public Semester Semester { get; set; } = null!;
}