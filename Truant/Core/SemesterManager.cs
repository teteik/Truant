using Microsoft.EntityFrameworkCore;
using Truant.Entities;
using Truant.History;
using Truant.Rules;
using Truant.Shared.Models;
using Truant.Shared.Utils;

namespace Truant.Core;

public class SemesterManager(TruantDbContext context, IRandomProvider random)
{
    public async Task<SemesterContext> CreateNewAsync()
    {
        var baseSeed = random.Next(int.MinValue, int.MaxValue);

        var semester = new Semester
        {
            RandomSeed = baseSeed,
            Status = 0 
        };
        context.Semesters.Add(semester);
        await context.SaveChangesAsync();
        
        var professors = new List<Professor>();
        var professorConfigs = new List<ProfessorConfig>();

        for (int i = 0; i < 6; i++)
        {
            var subject = (Subject)i;
            var ruleType = random.Next(1, 4);
            
            string? dependencies = null;
            Professor coreProfessor;

            if (ruleType == 1)
            {
                coreProfessor = new Professor(subject, new RandomRule(random));
            }
            else if (ruleType == 2)
            {
                var tempRule = new SingleDependencyRule(random);
                coreProfessor = new Professor(subject, tempRule);
                dependencies = tempRule.DependencySubject.ToString();
            }
            else 
            {
                var tempRule = new XorDependencyRule(random);
                coreProfessor = new Professor(subject, tempRule);
                dependencies = $"{tempRule.DependencyA},{tempRule.DependencyB}";
            }

            professorConfigs.Add(new ProfessorConfig
            {
                SemesterId = semester.Id,
                Subject = subject.ToString(),
                RuleType = ruleType,
                Dependencies = dependencies
            });
            
            professors.Add(coreProfessor);
        }

        context.ProfessorConfigs.AddRange(professorConfigs);
        await context.SaveChangesAsync();

        var history = new DatabaseStudentHistory(context, semester.Id, new List<AttendanceRecord>(), new List<QuestionRecord>());
        return new SemesterContext(semester.Id, professors, history, startDay: 1, startPleasure: 0, baseSeed);
    }

    public async Task<SemesterContext> ResumeAsync(long semesterId)
    {
        var semester = await context.Semesters
            .Include(s => s.ProfessorConfigs)
            .FirstOrDefaultAsync(s => s.Id == semesterId) 
            ?? throw new InvalidOperationException($"Семестр {semesterId} не найден.");

        if (semester.Status != 0)
            throw new InvalidOperationException($"Невозможно возобновить семестр со статусом {semester.Status}.");

        var lastStats = await context.DailyStatistics
            .Where(ds => ds.SemesterId == semesterId)
            .OrderByDescending(ds => ds.Day)
            .FirstOrDefaultAsync();

        var startDay = (lastStats?.Day ?? 0) + 1;
        var startPleasure = lastStats?.TotalPleasure ?? 0;

        var professors = new List<Professor>();
        foreach (var config in semester.ProfessorConfigs)
        {
            var subject = Enum.Parse<Subject>(config.Subject);
            Professor coreProfessor;

            if (config.RuleType == 1)
            {
                coreProfessor = new Professor(subject, new RandomRule(random));
            }
            else if (config.RuleType == 2)
            {
                var dep = Enum.Parse<Subject>(config.Dependencies!);
                coreProfessor = new Professor(subject, new SingleDependencyRule(dep));
            }
            else
            {
                var deps = config.Dependencies!.Split(',');
                var depA = Enum.Parse<Subject>(deps[0]);
                var depB = Enum.Parse<Subject>(deps[1]);
                coreProfessor = new Professor(subject, new XorDependencyRule(depA, depB));
            }
            professors.Add(coreProfessor);
        }

        var attendanceHistory = await context.AttendanceRecords
            .Where(a => a.SemesterId == semesterId).ToListAsync();
        var questionHistory = await context.QuestionRecords
            .Where(q => q.SemesterId == semesterId).ToListAsync();

        var history = new DatabaseStudentHistory(context, semester.Id, attendanceHistory, questionHistory);

        return new SemesterContext(semester.Id, professors, history, startDay, startPleasure, semester.RandomSeed);
    }
    
    public async Task RecordDayStatisticsAsync(long semesterId, int day, int totalPleasure, DayOutcome outcome)
    {
        var statistics = new DailyStatistics
        {
            SemesterId = semesterId,
            Day = day,
            TotalPleasure = totalPleasure,
            Outcome = (int)outcome
        };
        context.DailyStatistics.Add(statistics);
        await context.SaveChangesAsync();
    }

    public async Task UpdateStatusAsync(long semesterId, int status)
    {
        var semester = await context.Semesters.FindAsync(semesterId);
        if (semester != null)
        {
            semester.Status = status;
            await context.SaveChangesAsync();
        }
    }
}