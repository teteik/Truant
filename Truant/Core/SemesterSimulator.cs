using Truant.History;
using Truant.Rules;
using Truant.Strategies;
using Truant.Utils;

namespace Truant.Core;

public class SemesterSimulator(IRandomProvider random, IStudentStrategy strategy)
{
    public SimulationResult Run()
    {
        var professors = GenerateProfessors();
        var history = new SemesterHistory();
        
        var score = 100;
        var totalSkips = 0;

        for (int day = 1; day <= 100; day++)
        {
            var decisions = strategy.DecideDay(day, history);
            var dayOutcomes = new SubjectOutcome[6];
            
            var yesterday = history.GetDayOutcomes(day - 1);
            /*Console.Write($"Yesterday: ");
            for (int i = 0; i < 6; i++)
            {
                Console.Write(yesterday[i].WasAsked + " ");
            }
            Console.WriteLine();
            Console.WriteLine($"Decisions: {string.Join(" ",decisions)}");
*/
            for (int i = 0; i < 6; i++)
            {
                var attended = decisions[i];
                var wasAsked = professors[i].Ask(yesterday);
                
                dayOutcomes[i] = new SubjectOutcome(attended, wasAsked);

                if (!attended && wasAsked)
                {
                    return new SimulationResult(0, true, day, totalSkips);
                }

                if (attended) continue;
                totalSkips++;
                score++;
            }

            history.RecordDay(dayOutcomes);
            strategy.OnDayCompleted(day, history);
        }

        return new SimulationResult(score, false, 0, totalSkips);
    }

    private List<Professor> GenerateProfessors()
    {
        var professors = new List<Professor>();
        for (int i = 0; i < 6; i++)
        {
            var subject = (Subject)i;
            var ruleType = random.Next(1, 4);

            IQuestioningRule rule = ruleType switch
            {
                1 => new RandomRule(random),
                2 => new SingleDependencyRule(random),
                3 => new XorDependencyRule(random),
                _ => throw new InvalidOperationException()
            };
            //Console.WriteLine($"Subject: {subject}, Rule: {rule}");
            professors.Add(new Professor(subject, rule));
        }
        //Console.WriteLine();
        return professors;
    }
}