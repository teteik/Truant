namespace Truant.Shared.Dto;

public record DaySummaryRequest
{
    public int Day { get; init; }
    public int TotalPleasure { get; init; }
    public int Outcome { get; init; }
}
