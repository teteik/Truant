namespace Truant.Utils;

public class DefaultRandomProvider : IRandomProvider
{
    public int Next(int minValue, int maxValue)
    {
        return Random.Shared.Next(minValue, maxValue);
    }
}