using Truant.Rules;

namespace Truant.Core;

public class Professor(Subject subject, IQuestioningRule rule)
{
    public Subject Subject { get; } = subject;
    public IQuestioningRule Rule { get; } = rule;

    public bool Ask(SubjectOutcome[] yesterday)
    {
        return Rule.ShouldAsk(Subject, yesterday);
    }
}