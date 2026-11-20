using Truant.Shared.Models;

namespace Truant.Shared.Utils;

public static class SubjectRandomizer
{
    public static Subject NextSubject(IRandomProvider random)
    {
        return (Subject) random.Next(0, Enum.GetValues<Subject>().Length);    
    }

    public static (Subject A, Subject B) NextTwoDistinctSubjects(IRandomProvider random)
    {
        var a = NextSubject(random);
        var b = NextSubject(random);
        
        while (a == b) b = NextSubject(random);
        
        return (a, b);
    }
}