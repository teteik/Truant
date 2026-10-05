namespace Truant.Utils;

public class DefaultRandomProvider : IRandomProvider
{
    private Random _random = null!;
    public int Seed { get; private set; }
    public int CallCount { get; private set; }

    public DefaultRandomProvider() : this(Random.Shared.Next()) { }

    public DefaultRandomProvider(int seed) => RestoreState(seed, 0);
    public int Next(int minValue, int maxValue)
    {
        CallCount++;
        return _random.Next(minValue, maxValue);
    }

    public void RestoreState(int seed, int callCount)
    {
        Seed = seed;
        CallCount = callCount;
        _random = new Random(seed);
        for (int i = 0; i < callCount; i++)
            _random.Next();
    }
}