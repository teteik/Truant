namespace Truant.History;

public interface ISemesterHistory
{
    int CurrentDay { get; }
    bool Attended(int day, Subject subject);
    bool WasAsked(int day, Subject subject);
    void RecordDay(SubjectOutcome[] dayOutcomes);
    SubjectOutcome[] GetDayOutcomes(int day);
}