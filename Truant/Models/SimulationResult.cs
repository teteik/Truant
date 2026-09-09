namespace Truant;

public record SimulationResult
{
    public int FinalScore { get; init; }
    public bool IsExpelled { get; init; }
    public int ExpelledOnDay { get; init; }
    public int TotalSkips { get; init; }

    public SimulationResult(int finalScore, bool isExpelled, int expelledOnDay, int totalSkips)
    {
        FinalScore = finalScore;
        IsExpelled = isExpelled;
        ExpelledOnDay = expelledOnDay;
        TotalSkips = totalSkips;
    }
}