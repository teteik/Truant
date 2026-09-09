using System.IO.Pipes;
using Truant.Core;
using Truant.History;
using Truant.Rules;
using Truant.Utils;

namespace Truant;

public class AskingCounter
{
    private int _totalCount = 0;
    private int _wasAskedCount = 0;
    private int _wasAskedNotFirst = 0;
    private readonly IRandomProvider _random;
    private readonly int[] _rules = new int[3];

    public AskingCounter(IRandomProvider random)
    {
        _random = random;
    }
    
    public void RunProfessorsSimulation(int iterations = 10000)
    {
        var TotalCount = 0;
        var WasAsked = 0;
        var WasAskedNotFirst = 0;
        var rules = new int[3];

        for (int i = 0; i < iterations; i++)
        {
            var askingCounter = new AskingCounter(new DefaultRandomProvider());
            var TempTotal = 0;
            var TempWasAsked = 0;
            var TempWasAskedNotFirst = 0;
            var tempRules = new int[3];

            (TempTotal, TempWasAsked, TempWasAskedNotFirst, tempRules) = askingCounter.Run();

            TotalCount += TempTotal;
            WasAsked += TempWasAsked;
            WasAskedNotFirst += TempWasAskedNotFirst;

            for (int j = 0; j < 3; j++)
            {
                rules[j] += tempRules[j];
            }
        }

        Console.WriteLine($"Average Was Asked: {WasAsked / iterations}");
        Console.WriteLine($"Average Was Asked Not First: {WasAskedNotFirst / iterations}");
        Console.WriteLine($"Average score: {700 - 200 - WasAskedNotFirst / iterations}");

        Console.WriteLine();
        for (int i = 0; i < 3; i++)
        {
            Console.WriteLine($"Strategy {i + 1}: {rules[i]}");
        }
    }

    public (int, int, int, int[] ) Run()
    {
        var professors = GenerateProfessors();
        var history = new SemesterHistory();

        for (int day = 1; day <= 100; day++)
        {
            var dayOutcomes = new SubjectOutcome[6];
            
            var yesterday = history.GetDayOutcomes(day - 1);

            for (int i = 0; i < 6; i++)
            {
                var wasAsked = professors[i].Ask(yesterday);
                
                dayOutcomes[i] = new SubjectOutcome(true, wasAsked);

                if (wasAsked)
                {
                    _wasAskedCount++;
                    if (professors[i].Rule.RuleNumber != 1)
                        _wasAskedNotFirst++;
                }

                _totalCount++;
            }

            history.RecordDay(dayOutcomes);
        }
        
        
        
        return (_totalCount, _wasAskedCount, _wasAskedNotFirst,  _rules);
    }

    private List<Professor> GenerateProfessors()
    {
        var professors = new List<Professor>();
        for (int i = 0; i < 6; i++)
        {
            var subject = (Subject)i;
            var ruleType = _random.Next(1, 4);
            
            _rules[ruleType - 1]++;

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