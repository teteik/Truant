using Truant.History;

namespace Truant.Strategies;

public interface IStudentStrategy
{
    string Name { get; }
    bool[] DecideDay(int day, ISemesterHistory history);
    void OnDayCompleted(int day, ISemesterHistory history) { }
    void Reset() { }
}