using Truant.Entities;

namespace Truant.History;

public class DatabaseStudentHistory(TruantDbContext context, long simulationId) : IReadOnlyStudentHistory
{
    public bool Attended(int day, Subject subject)
    {
        var professorId = GetProfessorId(subject);
        
        if (professorId == 0) return false;
        
        var entry = GetDailyHistory(day, professorId);
        
        return entry?.Attended ?? false;
    }

    public bool? WasAsked(int day, Subject subject)
    {
        var professorId = GetProfessorId(subject);
        
        if (professorId == 0) return null;
        
        var entry = GetDailyHistory(day, professorId);
        
        return entry?.WasAsked;
    }

    private long GetProfessorId(Subject subject)
    {
        return context.Professors
            .Where(p => p.SimulationId == simulationId && p.Subject == subject.ToString())
            .Select(p => p.Id)
            .FirstOrDefault();
    }

    private DailyHistory? GetDailyHistory(int day, long professorId)
    {
        return context.DailyHistories
            .FirstOrDefault(h => h.SimulationId == simulationId 
                            && h.Day == day
                            && h.ProfessorId == professorId);
    }
}