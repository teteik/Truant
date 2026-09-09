using Truant.Utils;

namespace Truant.Rules;

public class SingleDependencyRule(IRandomProvider random) : IQuestioningRule
{
    private readonly Subject _dependencySubject = SubjectRandomizer.NextSubject(random);
    public int RuleNumber { get; } = 2;

    public bool ShouldAsk(Subject subject, SubjectOutcome[] yesterday)
    {
        return yesterday[(int) _dependencySubject].WasAsked;
    }
}