namespace Truant.Utils;

public class DefaultRandomProvider() : IRandomProvider
{
    private Random _random = new();

    public void Reset(int seed)
    {
        _random = new Random(seed);
    }
    public int Next(int minValue, int maxValue)
    {
        return Random.Shared.Next(minValue, maxValue);
    }
}