using Truant.History;

namespace Truant.Core;

public class SemesterContext(
    long semesterId,
    List<Professor> professors,
    IStudentHistory history,
    int startDay,
    int startPleasure,
    int baseRandomSeed)
{
    public long SemesterId { get; } = semesterId;
    public List<Professor> Professors { get; } = professors;
    public IStudentHistory History { get; } = history;
    public int StartDay { get; } = startDay;
    public int StartPleasure { get; } = startPleasure;
    public int BaseRandomSeed { get; } = baseRandomSeed;
}