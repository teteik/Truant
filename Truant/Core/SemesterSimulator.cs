using Truant.History;
using Truant.Rules;
using Truant.Strategies;
using Truant.Strategy;
using Truant.Utils;

namespace Truant.Core;

public class SemesterSimulator
{
    private readonly IRandomProvider _random;
    private readonly ISkipStrategy _strategy;
    private readonly SemesterHistory _history;
    private readonly List<Professor> _professors;

    private int CurrentDay { get; set; } = 1;
    private int TotalPleasure { get; set; } = 0;

    public SemesterSimulator(IRandomProvider random, ISkipStrategy strategy)
    {
        _random = random ?? throw new ArgumentNullException(nameof(random));
        _strategy = strategy ??  throw new ArgumentNullException(nameof(strategy));
        _history = new SemesterHistory();
        _professors = GenerateProfessors();
    }
    
    public DaySimulationResult Run()
    { 
        var daySimulationResult = SimulateDay();

        while (daySimulationResult.Outcome == DayOutcome.Continued)
        {
            daySimulationResult = SimulateDay();
            if (daySimulationResult.Outcome == DayOutcome.Expelled)
                return daySimulationResult;
        }
        
        return daySimulationResult;
    }

    public DaySimulationResult SimulateDay()
    {
        if (CurrentDay > 100)
            return new DaySimulationResult(DayOutcome.SemesterCompleted, CurrentDay - 1, TotalPleasure);

        var decisions = _strategy.DecideDay(CurrentDay, _history);
        var yesterday = _history.GetDayOutcomes(CurrentDay - 1);
        var subjectOutcomes = new SubjectOutcome[6];

        for (int i = 0; i < 6; i++)
        {
            var attended = decisions[i];
            var wasAsked = _professors[i].Ask(yesterday);

            subjectOutcomes[i] = new SubjectOutcome(attended, wasAsked);

            if (!attended && wasAsked)
            {
                TotalPleasure = 0;
                return new DaySimulationResult(DayOutcome.Expelled, CurrentDay, TotalPleasure);
            }

            if (!attended) TotalPleasure++;
        }

        _history.RecordDay(subjectOutcomes);
        TotalPleasure++;
        CurrentDay++;
        
        return new DaySimulationResult(DayOutcome.Continued,  CurrentDay - 1, TotalPleasure);
    }

    private List<Professor> GenerateProfessors()
    {
        var professors = new List<Professor>();
        for (int i = 0; i < 6; i++)
        {
            var subject = (Subject)i;
            var ruleType = _random.Next(1, 4);

            IQuestioningRule rule = ruleType switch
            {
                1 => new RandomRule(_random),
                2 => new SingleDependencyRule(_random),
                3 => new XorDependencyRule(_random),
                _ => throw new InvalidOperationException()
            };
            professors.Add(new Professor(subject, rule));
        }
        return professors;
    }
}