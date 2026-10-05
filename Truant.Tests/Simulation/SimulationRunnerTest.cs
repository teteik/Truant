using Truant.Core;
using Truant.History;
using Truant.Strategy;

namespace Truant.Tests.Simulation;

public class SimulationRunnerTest
{
    private class FakeStrategy : ISkipStrategy
    {
        public bool[] Yesterday { get; set; } = [false, false, false, false, false, false];
        
        public string Name { get; } = "FakeStrategy";

        public bool[] DecideDay(int day, IReadOnlyStudentHistory history)
        {
            return Yesterday;
        }
    }
    
    [Fact(DisplayName="If was asked and not attended, student is Expelled")]
    public void AskedNotAttended()
    {
        var random = new DeterministicRandom(1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 1);
        var strategy = new FakeStrategy();
        
        var simulator = new SemesterSimulator(random, strategy);

        var result = simulator.Run();
        
        Assert.Equal(DayOutcome.Expelled, result.Outcome);
        Assert.Equal(1, result.Day);
        Assert.Equal(0, result.TotalPleasure);
    }
    
    [Fact(DisplayName="If didn`t ask even once and didn`t attend even once, student is completed semester with maximum of pleasure")]
    public void NotAskedNotAttended()
    {
        var sequence = new int[111];
        for (int i = 0; i < 111; i++)
        {
            if (i % 2 == 0)
                sequence[i] = 1;
            else
                sequence[i] = 2;
            if (i > 10)
                sequence[i] = 0;
        }

        var random = new DeterministicRandom(sequence);
        var strategy = new FakeStrategy();
        
        var simulator = new SemesterSimulator(random, strategy);

        var result = simulator.Run();
        
        Assert.Equal(DayOutcome.SemesterCompleted, result.Outcome);
        Assert.Equal(100, result.Day);
        Assert.Equal(700, result.TotalPleasure);
    }
    
    [Fact(DisplayName="If didn`t ask even once and attend all subjects, student is completed semester with minimum (non zero) of pleasure")]
    public void NotAskedAttended()
    {
        var sequence = new int[111];
        for (int i = 0; i < 111; i++)
        {
            if (i % 2 == 0)
                sequence[i] = 1;
            else
                sequence[i] = 2;
            if (i > 10)
                sequence[i] = 0;
        }

        var random = new DeterministicRandom(sequence);
        var strategy = new FakeStrategy();
        strategy.Yesterday = [true, true, true, true, true, true];
        
        var simulator = new SemesterSimulator(random, strategy);

        var result = simulator.Run();
        
        Assert.Equal(DayOutcome.SemesterCompleted, result.Outcome);
        Assert.Equal(100, result.Day);
        Assert.Equal(100, result.TotalPleasure);
    }
    
    [Fact(DisplayName="If didn`t ask even once and attend all subjects, student is completed semester with minimum (non zero) of pleasure")]
    public void AskedAttended()
    {
        var sequence = new int[111];
        for (int i = 0; i < 111; i++)
        {
            if (i % 2 == 0)
                sequence[i] = 1;
            else
                sequence[i] = 2;
            if (i > 10)
                sequence[i] = 0;
        }

        var random = new DeterministicRandom(sequence);
        var strategy = new FakeStrategy();
        strategy.Yesterday = [true, true, true, true, true, true];
        
        var simulator = new SemesterSimulator(random, strategy);

        var result = simulator.Run();
        
        Assert.Equal(DayOutcome.SemesterCompleted, result.Outcome);
        Assert.Equal(100, result.Day);
        Assert.Equal(100, result.TotalPleasure);
    }
}