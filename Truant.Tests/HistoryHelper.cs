using Truant.History;

namespace Truant.Tests;

public class HistoryHelper(SubjectOutcome[] yesterday) : IReadOnlyStudentHistory
{
    public SubjectOutcome[] Yesterday = yesterday;

    public bool Attended(int day, Subject subject)
    {
        return Yesterday[(int) subject].Attended;
    }

    public bool? WasAsked(int day, Subject subject)
    {
        return Yesterday[(int) subject].WasAsked;
    }
}