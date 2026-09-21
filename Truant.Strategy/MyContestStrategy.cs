namespace Truant.Strategy;

public class MyContestStrategy : ISkipStrategy
{
    public string Name => "Artemev Contest Skip Strategy";
    
    private readonly int[,,] _combinations = new int[6, 6, 6];
    
    private int _lastLearnedDay = 0;

    public bool[] DecideDay(int day, IReadOnlyStudentHistory history)
    {
        LearnFromHistory(day - 1, history);

        if (day < 11) 
        {
            return [true, true, true, true, true, true];
        }

        var decisions = new bool[6];
        for (int i = 0; i < 6; i++)
        {
            var subject = (Subject)i;
            var probability = Predict(subject, day, history);
            
            decisions[i] = probability > 0.05;
        }
        
        return decisions;
    }

    private void LearnFromHistory(int upToDay, IReadOnlyStudentHistory history)
    {
        for (int d = _lastLearnedDay + 1; d <= upToDay; d++)
        {
            UpdateCombinations(d, history);
        }
        _lastLearnedDay = Math.Max(_lastLearnedDay, upToDay);
    }

    private double Predict(Subject subject, int day, IReadOnlyStudentHistory history)
    {
        double totalWeight = 0, weightedSum = 0;

        var weightRand = Math.Pow(0.5, day);
        totalWeight += weightRand;
        weightedSum += 0.5 * weightRand;
        
        for (int i = 0; i < 6; i++)
        {
            if (_combinations[(int)subject, i, i] == 0) 
            {
                var askedI = history.WasAsked(day - 1, (Subject)i);
                
                if (askedI.HasValue) 
                {
                    totalWeight += 1;
                    weightedSum += askedI.Value ? 1.0 : 0.0;
                }
            }
        }

        for (int i = 0; i < 6; i++)
        {
            for (int j = i + 1; j < 6; j++)
            {
                if (_combinations[(int)subject, i, j] == 0)
                {
                    var askedI = history.WasAsked(day - 1, (Subject)i);
                    var askedJ = history.WasAsked(day - 1, (Subject)j);
                    
                    if (askedI.HasValue && askedJ.HasValue)
                    {
                        totalWeight += 1;
                        weightedSum += (askedI.Value ^ askedJ.Value) ? 1.0 : 0.0;
                    }
                }
            }
        }
        
        return weightedSum / totalWeight;
    }

    private void UpdateCombinations(int day, IReadOnlyStudentHistory history)
    {
        for (int subjectIdx = 0; subjectIdx < 6; subjectIdx++)
        {
            var subject = (Subject)subjectIdx;
            var actualAsked = history.WasAsked(day, subject);

            if (!actualAsked.HasValue)
                continue;

            for (int i = 0; i < 6; i++)
            {
                var prevI = history.WasAsked(day - 1, (Subject)i);

                if (prevI.HasValue)
                {
                    if (_combinations[subjectIdx, i, i] == 0 && actualAsked.Value != prevI.Value)
                        _combinations[subjectIdx, i, i] = 1; 
                }

                for (int j = i + 1; j < 6; j++)
                {
                    var prevJ = history.WasAsked(day - 1, (Subject)j);

                    if (prevI.HasValue && prevJ.HasValue)
                    {
                        bool expectedXor = prevI.Value ^ prevJ.Value;
                        if (_combinations[subjectIdx, i, j] == 0 && actualAsked.Value != expectedXor)
                            _combinations[subjectIdx, i, j] = 1; 
                    }
                }
            }
        }
    }
}