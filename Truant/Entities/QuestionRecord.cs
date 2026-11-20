namespace Truant.Entities;

public class QuestionRecord
{
    public long Id { get; set; }
    public long SemesterId { get; set; }
    public int Day { get; set; }
    public string Subject { get; set; } = string.Empty;
    public bool? WasAsked { get; set; } 

    public Semester Semester { get; set; } = null!;
}