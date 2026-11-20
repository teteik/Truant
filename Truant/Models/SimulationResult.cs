namespace Truant.Models;

public record SimulationResult(int FinalScore, bool IsExpelled, int ExpelledOnDay, int TotalSkips);