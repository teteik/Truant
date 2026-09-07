namespace Truant;

public record SubjectOutcome
{
    public bool Attended { get; init; }
    public bool WasAsked { get; init; }
}