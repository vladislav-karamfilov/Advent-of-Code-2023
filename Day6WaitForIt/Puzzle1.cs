namespace Day6WaitForIt;

internal static class Puzzle1
{
    // https://adventofcode.com/2023/day/6
    public static void Solve()
    {
        var (raceDurationsInMs, recordRaceDistancesInMm) = ReadInput();

        var product = 1;
        for (var i = 0; i < raceDurationsInMs.Count; i++)
        {
            var raceMilliseconds = raceDurationsInMs[i];
            var recordMillimeters = recordRaceDistancesInMm[i];

            var winningWays = 0;
            for (var chargeMs = 1; chargeMs < raceMilliseconds; chargeMs++)
            {
                var moveMs = raceMilliseconds - chargeMs;
                var movedDistanceInMm = moveMs * chargeMs;

                if (movedDistanceInMm > recordMillimeters)
                {
                    winningWays++;
                }
            }

            product *= winningWays;
        }

        Console.WriteLine(product);
    }

    private static (List<int> RaceDurationsInMs, List<int> RecordRaceDistancesInMm) ReadInput()
    {
        var raceDurationsInMsLine = Console.ReadLine()!;
        var durationsStartIndex = raceDurationsInMsLine.IndexOf(':') + 1;
        var raceDurationsInMs = raceDurationsInMsLine[durationsStartIndex..]
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .ToList();

        var recordRaceDistancesInMmLine = Console.ReadLine()!;
        var distancesStartIndex = recordRaceDistancesInMmLine.IndexOf(':') + 1;
        var recordRaceDistancesInMm = recordRaceDistancesInMmLine[distancesStartIndex..]
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .ToList();

        return (raceDurationsInMs, recordRaceDistancesInMm);
    }
}
