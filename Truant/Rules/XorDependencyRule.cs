using Truant.Utils;

namespace Truant.Rules;

public class XorDependencyRule : IQuestioningRule
{
    private readonly Subject _dependencyA;
    private readonly Subject _dependencyB;

    public int RuleNumber { get; } = 3;

    public XorDependencyRule(IRandomProvider random)
    {
        (_dependencyA, _dependencyB) = SubjectRandomizer.NextTwoDistinctSubjects(random);
    }

    public bool ShouldAsk(Subject subject, SubjectOutcome[] yesterday)
    {
        return yesterday[(int) _dependencyA].WasAsked ^ yesterday[(int) _dependencyB].WasAsked;
    }
}
