namespace Truant.Rules;

public interface IQuestioningRule
{
    bool ShouldAsk(Subject subject, SubjectOutcome[] yesterday);
}