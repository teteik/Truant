using Truant.Shared.Models;

namespace Truant.History;

public interface IStudentHistory : IReadOnlyStudentHistory
{
    void RecordDay(SubjectOutcome[] dayOutcomes);

    SubjectOutcome[] GetDayOutcomes(int day);
}