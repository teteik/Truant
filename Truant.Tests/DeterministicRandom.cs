using Truant.Utils;

namespace Truant.Tests;

public class DeterministicRandom(params int[] sequence) : IRandomProvider
{
    private int _index;

    public int Next(int min, int max)
    {
        if (_index >= sequence.Length)
            throw new IndexOutOfRangeException("The sequence has ended");
        var result = sequence[_index++];
        if (result < min || result >= max)
            throw new ArgumentOutOfRangeException(nameof(result), $"Значение {result} вне диапазона [{min}, {max})");
        
        return result;
    }
}