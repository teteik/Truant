using Truant.Strategies;
using Truant.Strategy;
using Truant.Utils;

namespace Truant.Core;

public class MultipleSimulationRunner(ISkipStrategy strategy)
{
    public void Run(int iterations = 10000)
    {
        var totalScore = 0;
        var loseCount = 0;
        var minScore = int.MaxValue;
        var maxScore = 0;

        for (int i = 0; i < iterations; i++)
        {
            var simulator = new SemesterSimulator(new DefaultRandomProvider(), strategy);
            var result = simulator.Run();

            totalScore += result.FinalScore;
            if (result.IsExpelled) loseCount++;
            
            if (result.FinalScore > maxScore) maxScore = result.FinalScore;
            if (result.FinalScore < minScore) minScore = result.FinalScore;
        }

        Console.WriteLine($"Strategy: {strategy.Name}");
        Console.WriteLine($"Iterations: {iterations}");
        Console.WriteLine($"Average score: {(double)totalScore / iterations:F2}");
        Console.WriteLine($"Lost count: {loseCount} ({(double)loseCount / iterations * 100:F1}%)");
        Console.WriteLine($"Best score: {maxScore}");
        Console.WriteLine($"Worst score: {minScore}");
    }
}
