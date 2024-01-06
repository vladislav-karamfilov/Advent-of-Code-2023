namespace Day9MirageMaintenance;

internal static class Puzzle2
{
    // https://adventofcode.com/2023/day/9#part2
    public static void Solve()
    {
        var sum = 0L;
        while (true)
        {
            var line = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(line))
            {
                break;
            }

            var numbers = line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();

            var sequences = CalculateSequences(numbers);

            sum += CalculateNextValue(sequences);
        }

        Console.WriteLine(sum);
    }

    private static int CalculateNextValue(List<List<int>> sequences)
    {
        var nextValue = 0;
        for (var i = sequences.Count - 2; i >= 0; i--)
        {
            var sequence = sequences[i];
            nextValue = sequence[0] - nextValue;
        }

        return nextValue;
    }

    private static List<List<int>> CalculateSequences(List<int> numbers)
    {
        var sequences = new List<List<int>> { numbers };

        var newSequence = numbers;
        while (newSequence.Any(n => n != 0))
        {
            newSequence = new List<int>(capacity: numbers.Count - 1);
            for (var i = 0; i < numbers.Count - 1; i++)
            {
                newSequence.Add(numbers[i + 1] - numbers[i]);
            }

            numbers = newSequence;
            sequences.Add(newSequence);
        }

        return sequences;
    }
}
