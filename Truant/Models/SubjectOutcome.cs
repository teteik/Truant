namespace Truant;

public record SubjectOutcome
{
    public bool Attended { get; init; }
    public bool WasAsked { get; init; }

    public SubjectOutcome(bool attended, bool wasAsked)
    {
        Attended = attended;
        WasAsked = wasAsked;
    }
}