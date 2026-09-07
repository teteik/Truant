namespace Truant;


class Strategy
{
    public int Subject { get; init; }
    public int StrategyNumber { get; init; }
    public int[] Dependencies { get; init; }
    
    public int[][] Combinations { get; set; } = new int[6][];

    public Strategy(int subject, int strategyNumber, int[] dependencies)
    {
        for (int i = 0; i < 6; i++)
        {
            Combinations[i] = new int[6];
        }
        
        Subject = subject;
        StrategyNumber = strategyNumber;
        Dependencies = dependencies;
    }

    public void PrintCombinations()
    {
        for (int i = 0; i < 6; i++)
        {
            for (int j = i; j < 6; j++)
            {
                Console.Write(Combinations[i][j] + " ");
            }
            Console.WriteLine();
        }
    }
}