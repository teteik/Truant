using Truant.History;

namespace Truant.Strategies;

public class FirstSkipStrategy : IStudentStrategy
{
    public string Name => "First Skip Strategy";
    private readonly int[,,] _combinations = new int[6, 6, 6];

    public bool[] DecideDay(int day, ISemesterHistory history)
    {
        if (day == 1)
            return [true, true, true, true, true, true];

        var decisions = new bool[6];
        for (int i = 0; i < 6; i++)
        {
            var subject = (Subject)i;
            var probability = Predict(subject, day, history);
            
            if (probability > 0.01)
            {
                decisions[i] = true;
            }
            else
                decisions[i] = false;
            if (day < 12)
                decisions[i] = true;
        }
        
        return decisions;
    }
    
    private double Predict(Subject subject, int day, ISemesterHistory history)
    {
        double totalWeight = 0, weightedSum = 0;

        double weightRand = Math.Pow(0.5, day);
        totalWeight += weightRand;
        weightedSum += 0.5 * weightRand;
        
        for (int i = 0; i < 6; i++)
        {
            if (_combinations[(int)subject, i, i] != 1)
            {
                totalWeight += 1;
                weightedSum += history.WasAsked(day - 1, (Subject)i) ? 1 : 0;
            }
        }

        for (int i = 0; i < 6; i++)
        {
            for (int j = i + 1; j < 6; j++)
            {
                if (_combinations[(int)subject, i, j] != 1)
                {
                    totalWeight += 1;
                    var subjectI = history.WasAsked(day - 1, (Subject)i) ? 1 : 0;
                    var subjectJ = history.WasAsked(day - 1, (Subject)j) ? 1 : 0;
                        
                    weightedSum += subjectI ^ subjectJ;
                }
            }
        }
        
        return weightedSum /  totalWeight;
    }
    
    public void OnDayCompleted(int day, ISemesterHistory history)
    {
        UpdateCombinations(day, history);
    }
    
    private void UpdateCombinations(int day, ISemesterHistory history)
    {
        for (int subjectIdx = 0; subjectIdx < 6; subjectIdx++)
        {
            var subject = (Subject)subjectIdx;
            var actualAsked = history.WasAsked(day, subject);

            for (int i = 0; i < 6; i++)
            {
                var prevI = history.WasAsked(day - 1, (Subject)i);

                if (_combinations[subjectIdx, i, i] != 1 && actualAsked != prevI)
                    _combinations[subjectIdx, i, i] = 1;

                for (int j = i + 1; j < 6; j++)
                {
                    var prevJ = history.WasAsked(day - 1, (Subject)j);

                    if (_combinations[subjectIdx, i, j] != 1 && actualAsked != (prevI ^ prevJ))
                        _combinations[subjectIdx, i, j] = 1;
                }
            }
        }
    }
    
    public void Reset()
    {
        Array.Clear(_combinations, 0, _combinations.Length);
    }
}