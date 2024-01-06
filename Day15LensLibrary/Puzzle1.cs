namespace Day15LensLibrary;

internal static class Puzzle1
{
    // https://adventofcode.com/2023/day/15
    public static void Solve()
    {
        var initializationSequence = Console.ReadLine(); // File.ReadLines("input.txt").FirstOrDefault();

        var sum = 0;
        if (string.IsNullOrWhiteSpace(initializationSequence))
        {
            Console.WriteLine(sum);
            return;
        }

        var steps = initializationSequence.Split(',');
        foreach (var step in steps)
        {
            sum += PuzzleHelper.CalculateHashValue(step);
        }

        // Allocation-free algorithm
        //var currentValue = 0;
        //for (var i = 0; i < initializationSequence.Length; i++)
        //{
        //    var ch = initializationSequence[i];
        //    if (ch == ',')
        //    {
        //        sum += currentValue;
        //        currentValue = 0;
        //    }
        //    else
        //    {
        //        currentValue += ch;
        //        currentValue *= 17;
        //        currentValue %= 256;
        //    }
        //}

        // sum += currentValue;

        Console.WriteLine(sum);
    }
}
