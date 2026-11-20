namespace Truant.Shared.Utils;

public interface IRandomProvider
{
    int Next(int min, int max);
    void Reset(int seed);
}