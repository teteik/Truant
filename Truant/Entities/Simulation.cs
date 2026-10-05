namespace Truant.Entities;

public class Simulation
{
    public long Id { get; set; }
    public int Day { get; set; }
    public int Pleasure { get; set; }
    public int Status { get; set; }
    public int RandomSeed { get; set; }
    public int RandomCallCount { get; set; }
}