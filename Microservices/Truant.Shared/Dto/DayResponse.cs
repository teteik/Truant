namespace Truant.Shared.Dto;

public record DayResponse
{
    public int Day { get; init; }
    public List<SubjectOutcomeResponse> Outcomes { get; init; } = new();
}