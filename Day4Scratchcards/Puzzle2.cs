namespace Day4Scratchcards;

internal static class Puzzle2
{
    // https://adventofcode.com/2023/day/4#part2
    public static void Solve()
    {
        var count = 0;
        var cardCopies = new Dictionary<int, int>();
        while (true)
        {
            var line = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(line))
            {
                break;
            }

            var cardIdStartIndex = line.IndexOf(' ');
            var cardIdEndIndex = line.IndexOf(':');
            var cardId = int.Parse(line.AsSpan()[(cardIdStartIndex + 1)..cardIdEndIndex]);

            var numbersParts = line[(cardIdEndIndex + 1)..].Split('|');
            var winningNumbers = numbersParts[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToHashSet();
            var playerNumbers = numbersParts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();

            var playerWinningNumbers = playerNumbers.Count(winningNumbers.Contains);

            cardCopies.TryGetValue(cardId, out var originalCardCopies);
            for (var i = 0; i <= originalCardCopies; i++)
            {
                for (var j = cardId + 1; j <= cardId + playerWinningNumbers; j++)
                {
                    cardCopies.TryGetValue(j, out var copies);
                    cardCopies[j] = copies + 1;
                }
            }

            count += 1 + originalCardCopies;
        }

        Console.WriteLine(count);
    }
}
