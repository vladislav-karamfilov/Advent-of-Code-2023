namespace Day1Trebuchet;

using System.Buffers;

internal static class Puzzle2
{
    // https://adventofcode.com/2023/day/1#part2
    public static void Solve()
    {
        var digitSearchValues = SearchValues.Create("0123456789");
        var digitsAsWords = new Dictionary<string, char>
        {
            ["zero"] = '0',
            ["one"] = '1',
            ["two"] = '2',
            ["three"] = '3',
            ["four"] = '4',
            ["five"] = '5',
            ["six"] = '6',
            ["seven"] = '7',
            ["eight"] = '8',
            ["nine"] = '9',
        };

        var sum = 0L;
        while (true)
        {
            var line = Console.ReadLine();
            if (string.IsNullOrEmpty(line))
            {
                break;
            }

            var firstDigit = DetermineFirstDigit(line, digitSearchValues, digitsAsWords);
            var lastDigit = DetermineLastDigit(line, digitSearchValues, digitsAsWords);

            sum += int.Parse([firstDigit, lastDigit]);
        }

        Console.WriteLine(sum);
    }

    private static char DetermineFirstDigit(string line, SearchValues<char> digitSearchValues, Dictionary<string, char> digitsAsWords)
    {
        var firstDigit = default(char);
        var firstDigitIndex = line.AsSpan().IndexOfAny(digitSearchValues);
        if (firstDigitIndex == 0)
        {
            firstDigit = line[0];
        }
        else
        {
            var firstDigitWordResult = digitsAsWords
                .Select(d => new { Digit = d.Value, Index = line.IndexOf(d.Key, StringComparison.Ordinal) })
                .Where(x => x.Index >= 0)
                .MinBy(x => x.Index);

            if (firstDigitWordResult is not null && (firstDigitIndex == -1 || firstDigitWordResult.Index < firstDigitIndex))
            {
                firstDigit = firstDigitWordResult.Digit;
            }
            else if (firstDigitIndex != -1)
            {
                firstDigit = line[firstDigitIndex];
            }
        }

        return firstDigit;
    }

    private static char DetermineLastDigit(string line, SearchValues<char> digitSearchValues, Dictionary<string, char> digitsAsWords)
    {
        var lastDigit = default(char);
        var lastDigitIndex = line.AsSpan().LastIndexOfAny(digitSearchValues);
        if (lastDigitIndex == line.Length - 1)
        {
            lastDigit = line[^1];
        }
        else
        {
            var lastDigitWordResult = digitsAsWords
                .Select(d => new { Digit = d.Value, Index = line.LastIndexOf(d.Key, StringComparison.Ordinal) })
                .Where(x => x.Index >= 0)
                .MaxBy(x => x.Index);

            if (lastDigitWordResult?.Index > lastDigitIndex)
            {
                lastDigit = lastDigitWordResult.Digit;
            }
            else if (lastDigitIndex != -1)
            {
                lastDigit = line[lastDigitIndex];
            }
        }

        return lastDigit;
    }
}
