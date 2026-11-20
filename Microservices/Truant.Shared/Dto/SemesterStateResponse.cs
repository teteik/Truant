namespace Truant.Shared.Dto;

public record SemesterStateResponse
{
    public long Id { get; init; }
    public int CurrentDay { get; init; }
    public int TotalPleasure { get; init; }
    public int Status { get; init; }
}