namespace Truant.Tests;

public static class SubjectOutcomeHelper
{
    public static SubjectOutcome AttendedAsked { get; } = new SubjectOutcome(true, true);
    public static SubjectOutcome AttendedNotAsked { get; }= new SubjectOutcome(true, false);
    public static SubjectOutcome NotAttendedAsked { get; }= new SubjectOutcome(false, true);
    public static SubjectOutcome NotAttendedNotAsked { get; } = new SubjectOutcome(false, false);

    public static SubjectOutcome[] CreateNotAttendedNotAsked()
    {
        var day = new SubjectOutcome[Enum.GetValues<Subject>().Length];
        
        for (int i = 0; i < Enum.GetValues<Subject>().Length; i++)
        {
            day[i] = NotAttendedNotAsked;
        }
        
        return day;
    }
}