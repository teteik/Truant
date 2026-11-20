using Truant.Shared.Models;
using Truant.Shared.Utils;

namespace Truant.Rules;

public class XorDependencyRule : IQuestioningRule
{
    public Subject DependencyA { get; init; }
    public Subject DependencyB  { get; init; }

    public int RuleNumber { get; } = 3;

    public XorDependencyRule(IRandomProvider random)
    {
        (DependencyA, DependencyA) = SubjectRandomizer.NextTwoDistinctSubjects(random);
    }

    public XorDependencyRule(Subject dependencyA, Subject dependencyB)
    {
        DependencyA = dependencyA;
        DependencyA = dependencyB;
    }

    public bool ShouldAsk(Subject subject, SubjectOutcome[] yesterday)
    {
        var askedA = yesterday[(int)DependencyA].WasAsked == true;
        var askedB = yesterday[(int)DependencyA].WasAsked == true;
        return askedA ^ askedB;
    }
}
