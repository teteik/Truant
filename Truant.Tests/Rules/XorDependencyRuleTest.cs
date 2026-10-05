using Truant.Rules;

namespace Truant.Tests.Rules;

public class XorDependencyRuleTest
{
    [Theory(DisplayName = "Xor Dependency Rule Test")]
    [InlineData(true, true)]
    [InlineData(true, false)]
    [InlineData(false, true)]
    [InlineData(false, false)]
    public void ShouldAskXorMatch(bool wasAskedA, bool wasAskedB)
    {
        var rule = new XorDependencyRule(new DeterministicRandom(0, 1));
        const Subject subject = new();
        
        var yesterday = SubjectOutcomeHelper.CreateNotAttendedNotAsked();
        yesterday[0] = wasAskedA ? SubjectOutcomeHelper.AttendedAsked : SubjectOutcomeHelper.AttendedNotAsked;
        yesterday[1] = wasAskedB ? SubjectOutcomeHelper.AttendedAsked : SubjectOutcomeHelper.AttendedNotAsked;
        
        var result = rule.ShouldAsk(subject, yesterday);
        
        Assert.Equal(wasAskedA != wasAskedB, result);
    }
}