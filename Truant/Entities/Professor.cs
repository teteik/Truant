namespace Truant.Entities;

public class Professor
{
    public long Id { get; set; }
    public long SimulationId { get; set; }
    public string Subject { get; set; } = string.Empty;
    public int RuleNumber { get; set; }
}