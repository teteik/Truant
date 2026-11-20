namespace Truant.Shared.Dto;

public record SemesterResponse
{
    public long Id { get; init; }
    public int RandomSeed { get; init; }
    public int Status { get; init; }
}
