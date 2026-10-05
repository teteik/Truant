using Truant.Rules;

namespace Truant.Tests.Rules;

public class SingleDependencyRuleTests
{
    [Theory(DisplayName = "Single Dependency Rule Test")]
    [InlineData(true, true)]
    [InlineData(true, false)]
    [InlineData(false, true)]
    [InlineData(false, false)]
    public void ShouldAskIfAsked(bool attended, bool wasAsked)
    {
        var rule = new SingleDependencyRule(new DeterministicRandom(0, 1));
        const Subject subject = new();
        
        var yesterday = SubjectOutcomeHelper.CreateNotAttendedNotAsked();
        if (attended)
        {
            if (wasAsked)
                yesterday[0] = SubjectOutcomeHelper.AttendedAsked;
            else
                yesterday[0] = SubjectOutcomeHelper.AttendedNotAsked;
        }
        else
        {
            if (wasAsked)
                yesterday[0] = SubjectOutcomeHelper.NotAttendedAsked;
            else
                yesterday[0] = SubjectOutcomeHelper.NotAttendedNotAsked;
        }
        
        var result = rule.ShouldAsk(subject, yesterday);
        
        Assert.Equal(wasAsked, result);
    }
}