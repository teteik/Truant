namespace Truant;

public enum DayOutcome
{
    Continued,
    Expelled,
    SemesterCompleted
}

public record DaySimulationResult (DayOutcome Outcome, int Day, int TotalPleasure);