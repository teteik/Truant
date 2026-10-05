namespace Truant.Utils;

public interface IRandomProvider
{
    int Seed { get; }
    int CallCount { get; }
    
    int Next(int min, int max);
    void RestoreState(int seed, int callCount);
}