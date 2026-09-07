using Truant;

public class Program
{
    private static int[][] table = new int[101][];
    static Strategy[] strategies = new Strategy[6];
    private static int[] predicted = new int[6];
    private static int score = 100;
    private static int canSkip = 0;
    private static int launchCount = 0;

    public static void Main(string[] args)
    {
        var totalScore = 0;
        var loseCount = 0;
        var minScore = 700;
        var maxScore = 0;
        
        for (int i = 0; i < 10000; i++)
        {
            var launch = OneSemester();
            totalScore += launch;
            if (launch == 0)
                loseCount++;
            
            if (launch > maxScore)
                maxScore = launch;
            
            if (launch < minScore)
                minScore = launch;
        }

        double avgscore = (double)totalScore / 10000;
        double avgSkip = (double) canSkip / 10000;
        Console.WriteLine("Average score: {0}", avgscore);
        Console.WriteLine("Lost count: {0}", loseCount);
        Console.WriteLine("On average, You might have skipped: {0}", avgSkip);
        Console.WriteLine("Best score: {0}", maxScore);
        Console.WriteLine("Worst score: {0}", minScore);
    }
    
    public static int OneSemester()
    {
        for (int i = 0; i < table.Length; i++)
            table[i] = new int[6];
        CreateStrategies();

        launchCount++;
        
        score = 100;
        table[0] = [0, 0, 0, 0, 0, 0];
        for (int i = 1; i < 101; i++)
        {
            foreach (var strategy in strategies)
            {
                if (strategy.StrategyNumber == 1)
                {
                    SetNext(new DaySubject(i, strategy.Subject));
                }
                else if (strategy.StrategyNumber == 2)
                {
                    SetNext(new DaySubject(i, strategy.Subject), strategy.Dependencies[0]);
                }
                else
                {
                    SetNext(new DaySubject(i, strategy.Subject), strategy.Dependencies);
                }
                CheckDependencies(strategy, i);
                if (i >= 1)
                {
                    var pred = Predict(strategy, i);
                    if (pred > 0.9 || Math.Abs(pred - 0.5) < 0.02)
                    {
                        predicted[strategy.Subject] = 1;
                    }
                    else
                        predicted[strategy.Subject] = 0;
                }
                else
                {
                    predicted = [1, 1, 1, 1, 1, 1];
                }
            }

            for (int k = 0; k < 6; k++)
            {
                if (predicted[k] == 0 && predicted[k] != table[i][k])
                {
                    Console.WriteLine("YOU LOSE, DAY:" + i);
                    Console.WriteLine("Predicted: " + string.Join(", ", predicted));
                    Console.WriteLine("Get: " + string.Join(", ", table[i]));
                    return 0;
                }
                if (predicted[k] == 0 && predicted[k] == table[i][k])
                    score++;
                if (predicted[k] == 1 && predicted[k] != table[i][k])
                    canSkip++;
            }
        }

        if (score <= 200)
        {/*
            Console.WriteLine($"LOW SCORE at round {launchCount}, SCORE: {score}");
            foreach (var strategy in strategies)
            {
                Console.WriteLine($"[{strategy.Subject}] strategy{strategy.StrategyNumber}");
            }
            */
        }

        return score;
    }
    
    private static double Predict(Strategy strategy, int day)
    {
        double totalWeight = 0, weightedSum = 0;

        double weightRand = Math.Pow(0.5, day);
        totalWeight += weightRand;
        weightedSum += 0.5 * weightRand;
        
        for (int i = 0; i < 6; i++)
        {
            if (strategy.Combinations[i][i] != 1)
            {
                totalWeight += 1;
                weightedSum += table[day - 1][i];
            }
        }

        for (int i = 0; i < 6; i++)
        {
            for (int j = i + 1; j < 6; j++)
            {
                if (strategy.Combinations[i][j] != 1)
                {
                    totalWeight += 1;
                    weightedSum += table[day - 1][i] ^ table[day - 1][j];
                }
            }
        }
        
        return weightedSum /  totalWeight;
    }
    
    private static void CheckDependencies(Strategy strategy, int cur)
    {
        var subject = strategy.Subject;
        for (int i = 0; i < 6; i++)
        {
            if (table[cur][subject] != (table[cur - 1][i]))
                strategy.Combinations[i][i] = 1;
            for (int j = i + 1; j < 6; j++)
            {
                if (table[cur][subject] != (table[cur - 1][i] ^ table[cur - 1][j]))
                    strategy.Combinations[i][j] = 1;
            }
        }
    }

    private static void SetNext(DaySubject daySubject)
    {
        table[daySubject.Day][daySubject.Subject] = new Random().Next(0, 2);
    }
    
    private static void SetNext(DaySubject daySubject, int dependency)
    {
        table[daySubject.Day][daySubject.Subject] = table[daySubject.Day - 1][dependency];
    }
    
    private static void SetNext(DaySubject daySubject, int[] dependencies)
    {
        table[daySubject.Day][daySubject.Subject] = table[daySubject.Day - 1][dependencies[0]] ^ table[daySubject.Day - 1][dependencies[1]];
    }

    private static void CreateStrategies()
    {
        for (int i = 0; i < 6; i++)
        {
            var strategyNumber = new Random().Next(1, 4);
            int[] dependencies;
            if (strategyNumber == 1)
                dependencies = [];
            else if (strategyNumber == 2)
                dependencies = [ new Random().Next(0, 6) ];
            else 
            {
                var firstDependency = new Random().Next(0, 6);
                var secondDependency = new Random().Next(0, 6);
                
                while (firstDependency == secondDependency)
                    secondDependency = new Random().Next(0, 6);
                dependencies = [firstDependency, secondDependency];
            }

            strategies[i] = new Strategy(i, strategyNumber, dependencies);
        }
    }
    
    private class DaySubject
    {
        public int Day { get; set; }
        public int Subject { get; set; }

        public DaySubject(int day, int subject)
        {
            Day = day;
            Subject = subject;
        }
    }
}