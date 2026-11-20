using Truant.Shared.Models;
using Truant.Shared.Utils;

namespace Truant.Rules;

public class RandomRule(IRandomProvider random) : IQuestioningRule
{
    public int RuleNumber { get; set; } = 1;
    public bool ShouldAsk(Subject subject, SubjectOutcome[] yesterday)
    {
        return random.Next(0, 2) == 1;
    }
}