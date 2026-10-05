namespace Truant.Entities;

public class DailyHistory
{
    public long Id { get; set; }
    public long SimulationId { get; set; }
    public int Day { get; set; }
    public long ProfessorId { get; set; }
    public bool? WasAsked { get; set; }
    public bool Attended { get; set; }
}