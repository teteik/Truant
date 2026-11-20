using Truant.History;
using Truant.Shared.Models;
using Truant.Shared.Utils;
using Truant.Strategies;

namespace Truant.Core;

public class SemesterSimulator(
    IRandomProvider random,
    ISkipStrategy strategy,
    IStudentHistory history,
    List<Professor> professors,
    int startDay,
    int startPleasure,
    int baseSeed)
{
    private readonly IRandomProvider _random = random ?? throw new ArgumentNullException(nameof(random));
    private readonly ISkipStrategy _strategy = strategy ?? throw new ArgumentNullException(nameof(strategy));
    private readonly IStudentHistory _history = history ?? throw new ArgumentNullException(nameof(history));
    private readonly List<Professor> _professors = professors ?? throw new ArgumentNullException(nameof(professors));

    private int CurrentDay { get; set; } = startDay;
    private int TotalPleasure { get; set; } = startPleasure;

    public DaySimulationResult SimulateDay()
    {
        if (CurrentDay > 100)
            return new DaySimulationResult(DayOutcome.SemesterCompleted, CurrentDay - 1, TotalPleasure);

        _random.Reset(baseSeed + CurrentDay);

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
                for (int j = i + 1; j < 6; j++)
                {
                    subjectOutcomes[j] = new SubjectOutcome(false, null);
                }
                _history.RecordDay(subjectOutcomes);
                return new DaySimulationResult(DayOutcome.Expelled, CurrentDay, TotalPleasure);
            }

            if (!attended) TotalPleasure++;
        }

        _history.RecordDay(subjectOutcomes);
        TotalPleasure++;
        CurrentDay++;

        if (CurrentDay > 100)
            return new DaySimulationResult(DayOutcome.SemesterCompleted, 100, TotalPleasure);

        return new DaySimulationResult(DayOutcome.Continued, CurrentDay - 1, TotalPleasure);
    }
}