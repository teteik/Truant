using Truant.History;

namespace Truant.Strategies;

public class MathSkipStrategy : IStudentStrategy
{
    public string Name => "Math Skip Strategy";

    public bool[] DecideDay(int day, ISemesterHistory history)
    {
        return [false];
    }
    
}