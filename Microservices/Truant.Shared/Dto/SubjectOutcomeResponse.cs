namespace Truant.Shared.Dto;

public record SubjectOutcomeResponse
{
    public string Subject { get; init; } = string.Empty;
    public bool Attended { get; init; }
    public bool? WasAsked { get; init; }
}