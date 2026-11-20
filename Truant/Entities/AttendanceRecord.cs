namespace Truant.Entities;

public class AttendanceRecord
{
    public long Id { get; set; }
    public long SemesterId { get; set; }
    public int Day { get; set; }
    public string Subject { get; set; } = string.Empty;
    public bool Attended { get; set; }

    public Semester Semester { get; set; } = null!;
}