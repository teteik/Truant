namespace Truant.Shared.Dto;

public record AskRequest
{
    public int Day { get; init; }
    public bool Attended { get; init; }
}