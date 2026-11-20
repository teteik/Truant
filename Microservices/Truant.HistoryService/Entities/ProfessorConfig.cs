namespace Truant.HistoryService.Entities;

public class ProfessorConfig
{
    public long Id { get; set; }
    public long SemesterId { get; set; }
    public string Subject { get; set; } = string.Empty;
    public int RuleType { get; set; } 
    public string? Dependencies { get; set; }

    public Semester Semester { get; set; } = null!;
}