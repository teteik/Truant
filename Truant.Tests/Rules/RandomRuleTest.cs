using Truant.Rules;

namespace Truant.Tests.Rules;

public class RandomRuleTest
{
    [Fact(DisplayName = "Random Dependency Rule Test")]
    public void ShouldAskIfAsked()
    {
        var rule = new RandomRule(new DeterministicRandom(1, 0));
        const Subject subject = new();
        var yesterday = SubjectOutcomeHelper.CreateNotAttendedNotAsked();

        var result1 = rule.ShouldAsk(subject, yesterday);
        var result2 = rule.ShouldAsk(subject, yesterday);

        Assert.True(result1);
        Assert.False(result2);
    }
}

