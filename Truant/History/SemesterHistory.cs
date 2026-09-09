namespace Truant.History;

public class SemesterHistory : ISemesterHistory
{
    private readonly List<SubjectOutcome[]> _history = new();
    public int CurrentDay => _history.Count;

    public SemesterHistory()
    {
        var dayZeroOutcomes = new SubjectOutcome[Enum.GetValues<Subject>().Length];
        for (int i = 0; i < dayZeroOutcomes.Length; i++)
        {
            dayZeroOutcomes[i] = new SubjectOutcome(false, false);
        }
        _history.Add(dayZeroOutcomes);
    }

    public bool Attended(int day, Subject subject)
    {
        if (day < 0 || day >= _history.Count)
            throw new InvalidOperationException($"No day outcome found for day {day}");
        
        return _history[day][(int) subject].Attended;
    }

    public bool WasAsked(int day, Subject subject)
    {
        if (day < 0 || day >= _history.Count)
            throw new InvalidOperationException($"No day outcome found for day {day}");
        
        return _history[day][(int) subject].WasAsked;
    }

    public void RecordDay(SubjectOutcome[] dayOutcomes)
    {
        if (dayOutcomes.Length != Enum.GetValues<Subject>().Length)
            throw new ArgumentException($"The array must contain exactly  {Enum.GetValues<Subject>().Length} elements.");
        
        _history.Add(dayOutcomes);
    }

    public SubjectOutcome[] GetDayOutcomes(int day)
    {
        if (day < 0 || day >= _history.Count)
            throw new InvalidOperationException($"No day outcome found for day {day}");
        
        return _history[day];
    }
}