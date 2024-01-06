namespace Day4Scratchcards;
internal static class Puzzle1
{
    // https://adventofcode.com/2023/day/4
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

            var cardIdEndIndex = line.IndexOf(':');
            var numbersParts = line[(cardIdEndIndex + 1)..].Split('|');
            var winningNumbers = numbersParts[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToHashSet();
            var playerNumbers = numbersParts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();

            var playerWinningNumbers = playerNumbers.Count(winningNumbers.Contains);
            if (playerWinningNumbers > 0)
            {
                sum += (long)Math.Pow(2, playerWinningNumbers - 1);
            }
        }

        Console.WriteLine(sum);
    }
}
