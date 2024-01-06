namespace Day1Trebuchet;

using System.Buffers;

internal static class Puzzle1
{
    // https://adventofcode.com/2023/day/1
    public static void Solve()
    {
        var digitSearchValues = SearchValues.Create("0123456789");

        var sum = 0L;
        while (true)
        {
            var line = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(line))
            {
                break;
            }

            var firstDigitIndex = line.AsSpan().IndexOfAny(digitSearchValues);
            var lastDigitIndex = line.AsSpan().LastIndexOfAny(digitSearchValues);

            sum += int.Parse([line[firstDigitIndex], line[lastDigitIndex]]);
        }

        Console.WriteLine(sum);
    }
}
