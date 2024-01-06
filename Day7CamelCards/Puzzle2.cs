namespace Day7CamelCards;

internal static class Puzzle2
{
    // https://adventofcode.com/2023/day/7#part2
    public static void Solve()
    {
        var hands = new List<CardHand>();
        while (true)
        {
            var line = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(line))
            {
                break;
            }

            var handParts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            hands.Add(new CardHand(handParts[0], int.Parse(handParts[1]), respectJokerRule: true));
        }

        hands.Sort();

        var totalWinnings = 0L;
        for (var i = 0; i < hands.Count; i++)
        {
            var hand = hands[i];
            var rank = i + 1;

            totalWinnings += hand.Bid * rank;
        }

        Console.WriteLine(totalWinnings);
    }
}
