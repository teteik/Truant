using Truant.Utils;

namespace Truant.Rules;

public class RandomRule(IRandomProvider random) : IQuestioningRule
{
    public bool ShouldAsk(Subject subject, SubjectOutcome[] yesterday)
    {
        return random.Next(0, 2) == 1;
    }
}