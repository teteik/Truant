using Truant.Entities;
using Truant.Models;

namespace Truant.History;

public class DatabaseStudentHistory : IStudentHistory
{
    private readonly TruantDbContext _context;
    private readonly long _semesterId;

    private readonly Dictionary<(int Day, string Subject), bool> _attendanceCache = new();
    private readonly Dictionary<(int Day, string Subject), bool?> _questionCache = new();

    public DatabaseStudentHistory(
        TruantDbContext context,
        long semesterId,
        List<AttendanceRecord> attendanceHistory,
        List<QuestionRecord> questionHistory)
    {
        _context = context;
        _semesterId = semesterId;

        foreach (var record in attendanceHistory)
        {
            _attendanceCache[(record.Day, record.Subject)] = record.Attended;
        }

        foreach (var record in questionHistory)
        {
            _questionCache[(record.Day, record.Subject)] = record.WasAsked;
        }

        if (_attendanceCache.Count == 0)
        {
            for (int i = 0; i < 6; i++)
            {
                var subjectStr = ((Subject)i).ToString();
                _attendanceCache[(0, subjectStr)] = false;
                _questionCache[(0, subjectStr)] = null;
            }
        }
    }

    public bool Attended(int day, Subject subject)
    {
        return _attendanceCache.TryGetValue((day, subject.ToString()), out var attended) && attended;
    }

    public bool? WasAsked(int day, Subject subject)
    {
        _questionCache.TryGetValue((day, subject.ToString()), out var wasAsked);
        return wasAsked;
    }

    public void RecordDay(SubjectOutcome[] dayOutcomes)
    {
        var currentDay = _attendanceCache.Keys.Count != 0 ? _attendanceCache.Keys.Max(k => k.Day) + 1 : 1;

        var newAttendance = new List<AttendanceRecord>();
        var newQuestions = new List<QuestionRecord>();

        for (int i = 0; i < dayOutcomes.Length; i++)
        {
            var subjectStr = ((Subject)i).ToString();
            var outcome = dayOutcomes[i];

            newAttendance.Add(new AttendanceRecord
            {
                SemesterId = _semesterId,
                Day = currentDay,
                Subject = subjectStr,
                Attended = outcome.Attended
            });

            newQuestions.Add(new QuestionRecord
            {
                SemesterId = _semesterId,
                Day = currentDay,
                Subject = subjectStr,
                WasAsked = outcome.WasAsked
            });

            _attendanceCache[(currentDay, subjectStr)] = outcome.Attended;
            _questionCache[(currentDay, subjectStr)] = outcome.WasAsked;
        }

        _context.AttendanceRecords.AddRange(newAttendance);
        _context.QuestionRecords.AddRange(newQuestions);
        _context.SaveChanges();
    }

    public SubjectOutcome[] GetDayOutcomes(int day)
    {
        var outcomes = new SubjectOutcome[6];
        for (int i = 0; i < 6; i++)
        {
            var subjectStr = ((Subject)i).ToString();
            
            var attended = _attendanceCache.TryGetValue((day, subjectStr), out var att) && att;
            _questionCache.TryGetValue((day, subjectStr), out var asked);
            
            outcomes[i] = new SubjectOutcome(attended, asked);
        }
        return outcomes;
    }
}