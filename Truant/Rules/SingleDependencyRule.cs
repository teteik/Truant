using Truant.Models;
using Truant.Utils;

namespace Truant.Rules;

public class SingleDependencyRule : IQuestioningRule
{
    public Subject DependencySubject { get; init; }
    public int RuleNumber { get; } = 2;
    
    public SingleDependencyRule(IRandomProvider random)
    {
        DependencySubject = SubjectRandomizer.NextSubject(random);
    }
    public SingleDependencyRule(Subject dependencySubject)
    {
        DependencySubject = dependencySubject;
    }

    public bool ShouldAsk(Subject subject, SubjectOutcome[] yesterday)
    {
        return yesterday[(int) DependencySubject].WasAsked == true;
    }
}