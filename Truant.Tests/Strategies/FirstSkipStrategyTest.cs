using Truant.History;
using Truant.Strategies;

namespace Truant.Tests.Strategies;

public class FirstSkipStrategyTest
{
    private class MockHistory : IReadOnlyStudentHistory
    {
        private readonly Dictionary<int, SubjectOutcome[]> _days = new();

        public void SetDay(int day, SubjectOutcome[] outcomes)
        {
            _days[day] = outcomes;
        }

        public bool Attended(int day, Subject subject)
        {
            if (_days.TryGetValue(day, out var outcomes))
                return outcomes[(int)subject].Attended;
            return false;
        }

        public bool? WasAsked(int day, Subject subject)
        {
            if (_days.TryGetValue(day, out var outcomes))
                return outcomes[(int)subject].WasAsked;
            return null;
        }
    }

    
    [Theory(DisplayName = "Should attend all subjects in first day")]
    [InlineData(1, 0)]
    [InlineData(1, 1)]
    [InlineData(2, 3)]
    [InlineData(3, 5)]
    [InlineData(0, 5)]
    public void ShouldAttend_AllSubjectsFirstDay(int start, int end)
    {
        bool[] expected = [true, true, true, true, true, true];
        
        var strategy = new FirstSkipStrategy();
        var history = new HistoryHelper(SubjectOutcomeHelper.CreateNotAttendedNotAsked());
        for (int i = start; i <= end; i++)
        {
            history.Yesterday[i] = SubjectOutcomeHelper.AttendedAsked;
        }
        
        var result = strategy.DecideDay(1, history);
        
        Assert.Equal(expected, result);
    }
    
    [Theory(DisplayName = "Should skip subjects past eleven days")]
    [InlineData(1, 0)]
    [InlineData(1, 1)]
    [InlineData(2, 3)]
    [InlineData(3, 5)]
    [InlineData(0, 5)]
    public void ShouldSkip_AllSubjects(int start, int end)
    {
        bool[] expected = [false, false, false, false, false, false];
        for (int i = start; i <= end; i++)
        {
            expected[i] = true;
        }
        
        var strategy = new FirstSkipStrategy();
        var history = new HistoryHelper(SubjectOutcomeHelper.CreateNotAttendedNotAsked());
        for (int i = start; i <= end; i++)
        {
            history.Yesterday[i] = SubjectOutcomeHelper.AttendedAsked;
        }
        
        for (int i = 0; i < 11; i++)
            strategy.DecideDay(i, history);
        
        var result = strategy.DecideDay(12, history);
        
        Assert.Equal(expected, result);
    }
    
    [Fact(DisplayName = "Should find second strategy after eleven days")]
    public void ShouldFindSecondStrategy()
    {
        var strategy = new FirstSkipStrategy();
        var history = new MockHistory();
    
        var baseOutcomes = SubjectOutcomeHelper.CreateNotAttendedNotAsked();
    
        history.SetDay(0, baseOutcomes);
    
        for (int day = 1; day <= 11; day++)
        {
            var dayOutcomes = new SubjectOutcome[baseOutcomes.Length];
            Array.Copy(baseOutcomes, dayOutcomes, baseOutcomes.Length);

            if (day % 2 == 0)
            {
                dayOutcomes[0] = SubjectOutcomeHelper.AttendedAsked;
                dayOutcomes[1] = SubjectOutcomeHelper.AttendedNotAsked;
            }
            else 
            {
                dayOutcomes[0] = SubjectOutcomeHelper.AttendedNotAsked;
                dayOutcomes[1] = SubjectOutcomeHelper.AttendedAsked;
            }
        
            history.SetDay(day, dayOutcomes);
        }

        var result = strategy.DecideDay(12, history);
        
        var outcomes = SubjectOutcomeHelper.CreateNotAttendedNotAsked();
        outcomes[0] = SubjectOutcomeHelper.AttendedAsked;
        outcomes[1] = SubjectOutcomeHelper.AttendedNotAsked;
        history.SetDay(12, outcomes);
        
        var result1 = strategy.DecideDay(13, history);
        
        Assert.True(result[0]);
        Assert.True(result[1]);
        Assert.False(result1[0]);
        Assert.True(result1[1]);
    
        for (int i = 2; i < 6; i++)
        {
            Assert.False(result[i]);
            Assert.False(result1[i]);
        }
    }
    
    [Fact(DisplayName = "Should find third strategy after eleven days")]
    public void ShouldFindThirdStrategy()
    {
        var strategy = new FirstSkipStrategy();
        var history = new MockHistory();
    
        var baseOutcomes = SubjectOutcomeHelper.CreateNotAttendedNotAsked();
    
        history.SetDay(0, baseOutcomes);
    
        for (int day = 1; day <= 15; day++)
        {
            var dayOutcomes = new SubjectOutcome[baseOutcomes.Length];
            Array.Copy(baseOutcomes, dayOutcomes, baseOutcomes.Length);

            if (day % 4 == 0)
            {
                dayOutcomes[0] = SubjectOutcomeHelper.AttendedNotAsked;
                dayOutcomes[1] = SubjectOutcomeHelper.AttendedNotAsked;
                dayOutcomes[2] = SubjectOutcomeHelper.AttendedNotAsked;
            }
            else if (day % 4 == 1) 
            {
                dayOutcomes[0] = SubjectOutcomeHelper.AttendedNotAsked;
                dayOutcomes[1] = SubjectOutcomeHelper.AttendedAsked;
                dayOutcomes[2] = SubjectOutcomeHelper.AttendedNotAsked;
            }
            else if (day % 4 == 2)
            {
                dayOutcomes[0] = SubjectOutcomeHelper.AttendedAsked;
                dayOutcomes[1] = SubjectOutcomeHelper.AttendedNotAsked;
                dayOutcomes[2] = SubjectOutcomeHelper.AttendedAsked;
            }
            else
            {
                dayOutcomes[0] = SubjectOutcomeHelper.AttendedAsked;
                dayOutcomes[1] = SubjectOutcomeHelper.AttendedAsked;
                dayOutcomes[2] = SubjectOutcomeHelper.AttendedAsked;
            }
        
            history.SetDay(day, dayOutcomes);
        }

        var result12 = strategy.DecideDay(12, history);
        Assert.False(result12[0], "День 12: по предмету не спросят");
        
        var result13 = strategy.DecideDay(13, history);
        Assert.False(result13[0], "День 13: по предмету не спросят");
        
        var result14 = strategy.DecideDay(14, history);
        Assert.True(result14[0], "День 14: по предмету спросят");
        
        var result15 = strategy.DecideDay(15, history);
        Assert.True(result15[0], "День 15: по предмету спросят");
    }
}