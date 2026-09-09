namespace Truant.Rules;

public interface IQuestioningRule
{
    public int RuleNumber { get; }
    bool ShouldAsk(Subject subject, SubjectOutcome[] yesterday);
}